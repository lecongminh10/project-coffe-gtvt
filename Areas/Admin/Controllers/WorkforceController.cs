using System.Security.Claims;
using BaiTapLon.Data;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class WorkforceController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public WorkforceController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index(DateTime? from, DateTime? to)
    {
        var start = (from ?? DateTime.Today.AddDays(-7)).Date;
        var end = (to ?? DateTime.Today.AddDays(14)).Date;
        var query = _context.EmployeeSchedules.Include(s => s.User).Include(s => s.Shift)
            .Where(s => s.WorkDate >= start && s.WorkDate <= end);
        if (!User.IsInRole("Admin"))
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            query = query.Where(s => s.UserId == userId);
        }
        ViewBag.From = start.ToString("yyyy-MM-dd");
        ViewBag.To = end.ToString("yyyy-MM-dd");
        return View(new WorkforceViewModel
        {
            Schedules = await query.OrderByDescending(s => s.WorkDate).ThenBy(s => s.ShiftId).ToListAsync(),
            Staff = await _context.Users.Where(u => u.Role == "Staff" || u.Role == "Admin").OrderBy(u => u.FullName).ToListAsync(),
            Shifts = await _context.Shifts.OrderBy(s => s.StartTime).ToListAsync()
        });
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSchedule(EmployeeSchedule schedule)
    {
        schedule.WorkDate = schedule.WorkDate.Date;
        var exists = await _context.EmployeeSchedules.AnyAsync(s => s.UserId == schedule.UserId &&
            s.ShiftId == schedule.ShiftId && s.WorkDate == schedule.WorkDate);
        if (exists) TempData["ErrorMessage"] = "Nhân viên đã được phân ca này trong ngày đã chọn.";
        else
        {
            schedule.Status = "Scheduled";
            _context.EmployeeSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã phân ca làm việc.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int id)
    {
        var schedule = await AllowedSchedule(id);
        if (schedule == null) return NotFound();
        if (schedule.CheckInTime.HasValue) TempData["ErrorMessage"] = "Ca làm đã check-in.";
        else
        {
            schedule.CheckInTime = DateTime.Now;
            schedule.Status = "Working";
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Check-in thành công.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int id)
    {
        var schedule = await AllowedSchedule(id);
        if (schedule == null) return NotFound();
        if (!schedule.CheckInTime.HasValue || schedule.CheckOutTime.HasValue)
            TempData["ErrorMessage"] = "Cần check-in trước hoặc ca đã check-out.";
        else
        {
            schedule.CheckOutTime = DateTime.Now;
            schedule.Status = "Completed";
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Check-out thành công.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveShift(Shift shift)
    {
        if (shift.EndTime <= shift.StartTime || shift.HourlyWage < 0)
        {
            TempData["ErrorMessage"] = "Thời gian hoặc mức lương ca không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }
        if (shift.ShiftId == 0) _context.Shifts.Add(shift);
        else
        {
            var existing = await _context.Shifts.FindAsync(shift.ShiftId);
            if (existing == null) return NotFound();
            existing.ShiftName = shift.ShiftName;
            existing.StartTime = shift.StartTime;
            existing.EndTime = shift.EndTime;
            existing.HourlyWage = shift.HourlyWage;
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu ca làm việc.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule = await _context.EmployeeSchedules.FindAsync(id);
        if (schedule != null) { _context.EmployeeSchedules.Remove(schedule); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    private async Task<EmployeeSchedule?> AllowedSchedule(int id)
    {
        var schedule = await _context.EmployeeSchedules.FindAsync(id);
        if (schedule == null || User.IsInRole("Admin")) return schedule;
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return schedule.UserId == userId ? schedule : null;
    }
}
