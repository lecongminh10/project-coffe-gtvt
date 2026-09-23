using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapLon.Controllers;

public class ReviewController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public ReviewController(CoffeeShopDbContext context) => _context = context;

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int productId, string customerName, int rating, string? comment)
    {
        if (rating is < 1 or > 5 || string.IsNullOrWhiteSpace(customerName))
        {
            TempData["ReviewError"] = "Vui lòng nhập tên và chọn từ 1 đến 5 sao.";
            return RedirectToAction("Details", "Product", new { id = productId });
        }
        int? userId = null;
        if (User.Identity?.IsAuthenticated == true)
            userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _context.Reviews.Add(new Review
        {
            ProductId = productId, UserId = userId, CustomerName = customerName.Trim(),
            Rating = rating, Comment = comment?.Trim(), CreatedAt = DateTime.Now, IsApproved = false
        });
        await _context.SaveChangesAsync();
        TempData["ReviewSuccess"] = "Cảm ơn bạn! Đánh giá sẽ hiển thị sau khi được duyệt.";
        return RedirectToAction("Details", "Product", new { id = productId });
    }
}
