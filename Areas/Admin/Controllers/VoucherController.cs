using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class VoucherController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public VoucherController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Vouchers.OrderByDescending(v => v.EndDate).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Voucher voucher)
    {
        voucher.Code = voucher.Code.Trim().ToUpperInvariant();
        if (voucher.EndDate < voucher.StartDate || voucher.UsageLimit < voucher.UsedCount)
            ModelState.AddModelError("", "Thời hạn hoặc số lượt sử dụng không hợp lệ.");
        var duplicate = await _context.Vouchers.AnyAsync(v => v.Code == voucher.Code && v.VoucherId != voucher.VoucherId);
        if (duplicate) ModelState.AddModelError(nameof(voucher.Code), "Mã voucher đã tồn tại.");
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Index));
        }
        if (voucher.VoucherId == 0) _context.Vouchers.Add(voucher);
        else
        {
            var existing = await _context.Vouchers.FindAsync(voucher.VoucherId);
            if (existing == null) return NotFound();
            existing.Code = voucher.Code;
            existing.DiscountPercent = voucher.DiscountPercent;
            existing.MaxDiscountAmount = voucher.MaxDiscountAmount;
            existing.MinOrderAmount = voucher.MinOrderAmount;
            existing.StartDate = voucher.StartDate;
            existing.EndDate = voucher.EndDate;
            existing.UsageLimit = voucher.UsageLimit;
            existing.IsActive = voucher.IsActive;
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu voucher.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var voucher = await _context.Vouchers.FindAsync(id);
        if (voucher != null) { _context.Vouchers.Remove(voucher); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
