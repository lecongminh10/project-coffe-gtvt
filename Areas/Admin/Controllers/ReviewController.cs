using BaiTapLon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class ReviewController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public ReviewController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Reviews.Include(r => r.Product)
        .OrderBy(r => r.IsApproved).ThenByDescending(r => r.CreatedAt).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, bool approved)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null) { review.IsApproved = approved; await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null) { _context.Reviews.Remove(review); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
