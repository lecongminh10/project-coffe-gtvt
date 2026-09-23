using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Helpers;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;
using System.Data;

namespace BaiTapLon.Controllers;

public class CartController : Controller
{
    private const string CART_SESSION_KEY = "Cart";
    private readonly CoffeeShopDbContext _context;

    public CartController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    private List<CartItem> GetCart()
    {
        return HttpContext.Session.GetObjectFromJson<List<CartItem>>(CART_SESSION_KEY) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetObjectAsJson(CART_SESSION_KEY, cart);
    }

    // GET: /Cart
    public async Task<IActionResult> Index()
    {
        var cart = GetCart();
        var model = new CartViewModel
        {
            Items = cart
        };

        // Danh sách bàn trống để khách hàng chọn nếu dùng tại quán
        ViewBag.AvailableTables = await _context.CoffeeTables
            .Where(t => t.Status == "Available")
            .OrderBy(t => t.Area).ThenBy(t => t.TableName)
            .ToListAsync();

        return View(model);
    }

    // POST: /Cart/AddToCartAjax
    [HttpPost]
    public async Task<IActionResult> AddToCartAjax(int productId, int quantity = 1)
    {
        if (quantity < 1 || quantity > 100)
            return Json(new { success = false, message = "Số lượng phải từ 1 đến 100." });
        var product = await _context.Products.FindAsync(productId);
        if (product == null || !product.IsAvailable)
        {
            return Json(new { success = false, message = "Món không tồn tại hoặc tạm ngưng phục vụ." });
        }

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });
        }

        SaveCart(cart);
        int totalCount = cart.Sum(c => c.Quantity);

        return Json(new
        {
            success = true,
            totalCount = totalCount,
            message = $"Đã thêm {quantity}x \"{product.Name}\" vào giỏ hàng!"
        });
    }

    // POST: /Cart/UpdateQuantity
    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            SaveCart(cart);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Cart/RemoveItem
    [HttpPost]
    public IActionResult RemoveItem(int productId)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Cart/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CartViewModel model)
    {
        var cart = GetCart();
        if (cart == null || !cart.Any())
        {
            ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
            ViewBag.AvailableTables = await _context.CoffeeTables.Where(t => t.Status == "Available").ToListAsync();
            model.Items = new List<CartItem>();
            return View("Index", model);
        }

        if (!ModelState.IsValid)
        {
            model.Items = cart;
            ViewBag.AvailableTables = await _context.CoffeeTables.Where(t => t.Status == "Available").ToListAsync();
            return View("Index", model);
        }

        if (model.OrderType is not ("DineIn" or "TakeAway"))
            ModelState.AddModelError(nameof(model.OrderType), "Hình thức phục vụ không hợp lệ.");
        var paymentMethods = new[] { "Tiền mặt", "Chuyển khoản QR", "Ví MoMo" };
        if (!paymentMethods.Contains(model.PaymentMethod))
            ModelState.AddModelError(nameof(model.PaymentMethod), "Phương thức thanh toán không hợp lệ.");

        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        CoffeeTable? selectedTable = null;
        if (model.OrderType == "DineIn")
        {
            if (!model.TableId.HasValue)
                ModelState.AddModelError(nameof(model.TableId), "Vui lòng chọn bàn.");
            else
            {
                selectedTable = await _context.CoffeeTables.FirstOrDefaultAsync(t =>
                    t.TableId == model.TableId && t.Status == "Available");
                if (selectedTable == null)
                    ModelState.AddModelError(nameof(model.TableId), "Bàn không còn trống, vui lòng chọn bàn khác.");
            }
        }

        var subtotal = cart.Sum(c => c.TotalPrice);
        Voucher? voucher = null;
        decimal discount = 0;
        if (!string.IsNullOrWhiteSpace(model.VoucherCode))
        {
            var code = model.VoucherCode.Trim().ToUpperInvariant();
            var now = DateTime.Now;
            voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code && v.IsActive);
            if (voucher == null || now < voucher.StartDate || now > voucher.EndDate || voucher.UsedCount >= voucher.UsageLimit)
                ModelState.AddModelError(nameof(model.VoucherCode), "Mã giảm giá không tồn tại, hết hạn hoặc đã hết lượt.");
            else if (subtotal < voucher.MinOrderAmount)
                ModelState.AddModelError(nameof(model.VoucherCode), $"Đơn hàng tối thiểu {voucher.MinOrderAmount:N0}đ để dùng mã này.");
            else
                discount = Math.Min(subtotal * voucher.DiscountPercent / 100m, voucher.MaxDiscountAmount);
        }

        if (!ModelState.IsValid)
        {
            model.Items = cart;
            ViewBag.AvailableTables = await _context.CoffeeTables.Where(t => t.Status == "Available").ToListAsync();
            return View("Index", model);
        }

        // Tạo đơn hàng mới lưu vào MySQL
        var order = new Order
        {
            OrderDate = DateTime.Now,
            CustomerName = model.CustomerName,
            CustomerPhone = model.CustomerPhone,
            TableId = model.OrderType == "DineIn" ? model.TableId : null,
            TotalAmount = subtotal - discount,
            VoucherCode = voucher?.Code,
            DiscountAmount = discount,
            Status = "Pending",
            PaymentMethod = model.PaymentMethod,
            IsPaid = false,
            Notes = $"[{model.OrderType}] {model.Notes}"
        };

        // Nếu người dùng đã đăng nhập, liên kết với UserId
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                order.UserId = user.UserId;
            }
        }

        // Tạo danh sách OrderDetail
        foreach (var item in cart)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Price
            });
        }

        // Cập nhật trạng thái bàn nếu khách chọn ngồi tại bàn
        if (order.TableId.HasValue)
        {
            selectedTable!.Status = "Occupied";
        }

        if (voucher != null) voucher.UsedCount++;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        // Xóa giỏ hàng trong Session sau khi đặt thành công
        HttpContext.Session.Remove(CART_SESSION_KEY);

        return RedirectToAction(nameof(OrderSuccess), new { id = order.OrderId });
    }

    // GET: /Cart/OrderSuccess/5
    public async Task<IActionResult> OrderSuccess(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        return View(order);
    }
}
