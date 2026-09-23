using System.Text.RegularExpressions;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class NewsController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public NewsController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.News.Include(n => n.Author).OrderByDescending(n => n.PublishedAt).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(News article)
    {
        article.Slug = Slugify(string.IsNullOrWhiteSpace(article.Slug) ? article.Title : article.Slug);
        if (await _context.News.AnyAsync(n => n.Slug == article.Slug && n.NewsId != article.NewsId))
            article.Slug += $"-{DateTime.Now:yyyyMMddHHmmss}";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Thông tin bài viết không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }
        if (article.NewsId == 0)
        {
            article.AuthorId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            _context.News.Add(article);
        }
        else
        {
            var existing = await _context.News.FindAsync(article.NewsId);
            if (existing == null) return NotFound();
            existing.Title = article.Title; existing.Slug = article.Slug; existing.Summary = article.Summary;
            existing.Content = article.Content; existing.ImageUrl = article.ImageUrl;
            existing.PublishedAt = article.PublishedAt; existing.IsActive = article.IsActive;
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu bài viết.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.News.FindAsync(id);
        if (article != null) { _context.News.Remove(article); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    private static string Slugify(string value)
    {
        value = value.ToLowerInvariant().Normalize(System.Text.NormalizationForm.FormD);
        value = string.Concat(value.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark));
        value = value.Replace('đ', 'd');
        return Regex.Replace(Regex.Replace(value, @"[^a-z0-9]+", "-"), @"(^-|-$)", "");
    }
}
