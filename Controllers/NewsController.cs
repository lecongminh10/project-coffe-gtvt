using BaiTapLon.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Controllers;

public class NewsController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public NewsController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.News.Include(n => n.Author)
        .Where(n => n.IsActive && n.PublishedAt <= DateTime.Now)
        .OrderByDescending(n => n.PublishedAt).ToListAsync());

    [HttpGet("/News/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var article = await _context.News.Include(n => n.Author)
            .FirstOrDefaultAsync(n => n.Slug == slug && n.IsActive && n.PublishedAt <= DateTime.Now);
        if (article == null) return NotFound();
        article.ViewCount++;
        await _context.SaveChangesAsync();
        return View(article);
    }
}
