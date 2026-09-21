using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Helpers;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;

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
            .OrderBy(t => t.Area).ThenBy(t => t.TableName)
            .ToListAsync();

        return View(model);
    }

    // POST: /Cart/AddToCartAjax
    [HttpPost]
    public async Task<IActionResult> AddToCartAjax(int productId, int quantity = 1)
    {
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
            ViewBag.AvailableTables = await _context.CoffeeTables.ToListAsync();
            model.Items = new List<CartItem>();
            return View("Index", model);
        }

        if (!ModelState.IsValid)
        {
            model.Items = cart;
            ViewBag.AvailableTables = await _context.CoffeeTables.ToListAsync();
            return View("Index", model);
        }

        // Tạo đơn hàng mới lưu vào MySQL
        var order = new Order
        {
            OrderDate = DateTime.Now,
            CustomerName = model.CustomerName,
            CustomerPhone = model.CustomerPhone,
            TableId = model.OrderType == "DineIn" ? model.TableId : null,
            TotalAmount = cart.Sum(c => c.TotalPrice),
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
            var table = await _context.CoffeeTables.FindAsync(order.TableId.Value);
            if (table != null)
            {
                table.Status = "Occupied";
            }
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

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
