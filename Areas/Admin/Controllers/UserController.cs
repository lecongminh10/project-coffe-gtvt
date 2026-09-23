using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Helpers;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public UserController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Users.OrderBy(u => u.Role).ThenBy(u => u.FullName).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(AdminUserFormViewModel model)
    {
        var roles = new[] { "Admin", "Staff", "Customer" };
        if (!roles.Contains(model.Role)) ModelState.AddModelError(nameof(model.Role), "Vai trò không hợp lệ.");
        if (model.UserId == 0 && string.IsNullOrWhiteSpace(model.Password))
            ModelState.AddModelError(nameof(model.Password), "Mật khẩu là bắt buộc với tài khoản mới.");
        if (await _context.Users.AnyAsync(u => u.Username == model.Username && u.UserId != model.UserId))
            ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại.");
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Index));
        }
        if (model.UserId == 0)
        {
            _context.Users.Add(new User
            {
                Username = model.Username.Trim(), FullName = model.FullName.Trim(), Email = model.Email,
                PhoneNumber = model.PhoneNumber, Role = model.Role,
                PasswordHash = PasswordHelper.HashPassword(model.Password!), CreatedAt = DateTime.Now
            });
        }
        else
        {
            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null) return NotFound();
            user.Username = model.Username.Trim(); user.FullName = model.FullName.Trim();
            user.Email = model.Email; user.PhoneNumber = model.PhoneNumber; user.Role = model.Role;
            if (!string.IsNullOrWhiteSpace(model.Password)) user.PasswordHash = PasswordHelper.HashPassword(model.Password);
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu tài khoản.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id == currentId)
        {
            TempData["ErrorMessage"] = "Không thể xóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }
        var user = await _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return NotFound();
        var isReferenced = user.Orders.Count > 0
            || await _context.EmployeeSchedules.AnyAsync(s => s.UserId == id)
            || await _context.Reviews.AnyAsync(r => r.UserId == id)
            || await _context.Wishlists.AnyAsync(w => w.UserId == id)
            || await _context.News.AnyAsync(n => n.AuthorId == id);
        if (isReferenced)
        {
            TempData["ErrorMessage"] = "Không thể xóa tài khoản đang có dữ liệu liên quan; hãy đổi vai trò nếu cần.";
            return RedirectToAction(nameof(Index));
        }
        _context.Users.Remove(user); await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
