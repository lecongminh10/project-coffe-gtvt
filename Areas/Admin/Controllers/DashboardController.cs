using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models.ViewModels;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class DashboardController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public DashboardController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var totalOrders = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders
            .Where(o => o.Status == "Completed" || o.IsPaid)
            .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        var totalProducts = await _context.Products.CountAsync();
        var totalCustomers = await _context.Users.CountAsync(u => u.Role == "Customer");

        var occupiedTables = await _context.CoffeeTables.CountAsync(t => t.Status == "Occupied");
        var availableTables = await _context.CoffeeTables.CountAsync(t => t.Status == "Available");

        var recentOrders = await _context.Orders
            .Include(o => o.Table)
            .OrderByDescending(o => o.OrderDate)
            .Take(7)
            .ToListAsync();

        var topProducts = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.IsFeatured)
            .Take(5)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            TotalProducts = totalProducts,
            TotalCustomers = totalCustomers,
            OccupiedTables = occupiedTables,
            AvailableTables = availableTables,
            RecentOrders = recentOrders,
            TopProducts = topProducts
        };

        return View(model);
    }
}
