using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class OrderController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public OrderController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: /Admin/Order
    public async Task<IActionResult> Index(string? status)
    {
        var query = _context.Orders
            .Include(o => o.Table)
            .Include(o => o.OrderDetails)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
        ViewBag.CurrentStatus = status;

        return View(orders);
    }

    // GET: /Admin/Order/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var order = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(m => m.OrderId == id);

        if (order == null) return NotFound();

        return View(order);
    }

    // POST: /Admin/Order/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int orderId, string status, bool isPaid)
    {
        var order = await _context.Orders.Include(o => o.Table).FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Status = status;
            order.IsPaid = isPaid;

            // Nếu đơn hoàn thành hoặc hủy, giải phóng bàn về Available
            if ((status == "Completed" || status == "Cancelled") && order.Table != null)
            {
                order.Table.Status = "Available";
            }
            else if (status == "Processing" && order.Table != null)
            {
                order.Table.Status = "Occupied";
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái đơn #{order.OrderId} thành: {status}";
        }

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    // POST: /Admin/Order/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa đơn hàng #{id} khỏi hệ thống!";
        }
        return RedirectToAction(nameof(Index));
    }
}
