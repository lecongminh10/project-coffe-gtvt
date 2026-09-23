using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public WishlistController(CoffeeShopDbContext context) => _context = context;
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> Index() => View(await _context.Wishlists.Include(w => w.Product)
        .Where(w => w.UserId == UserId).OrderByDescending(w => w.CreatedAt).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int productId, string? returnUrl)
    {
        var item = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == UserId && w.ProductId == productId);
        if (item == null)
        {
            if (await _context.Products.AnyAsync(p => p.ProductId == productId))
                _context.Wishlists.Add(new Wishlist { UserId = UserId, ProductId = productId, CreatedAt = DateTime.Now });
        }
        else _context.Wishlists.Remove(item);
        await _context.SaveChangesAsync();
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return LocalRedirect(returnUrl);
        return RedirectToAction(nameof(Index));
    }
}
