using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models;

namespace BaiTapLon.Controllers;

public class HomeController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public HomeController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Lấy danh mục
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        // Lấy các món bán chạy / nổi bật
        var featuredProducts = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsAvailable && p.IsFeatured)
            .OrderByDescending(p => p.ProductId)
            .Take(6)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.FeaturedProducts = featuredProducts;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
