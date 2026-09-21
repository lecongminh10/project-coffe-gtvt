using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class TableController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public TableController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: /Admin/Table
    public async Task<IActionResult> Index()
    {
        var tables = await _context.CoffeeTables
            .OrderBy(t => t.Area)
            .ThenBy(t => t.TableName)
            .ToListAsync();

        return View(tables);
    }

    // POST: /Admin/Table/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int tableId, string status)
    {
        var table = await _context.CoffeeTables.FindAsync(tableId);
        if (table != null)
        {
            table.Status = status;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật {table.TableName} sang trạng thái: {status}";
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: /Admin/Table/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CoffeeTable table)
    {
        if (ModelState.IsValid)
        {
            _context.CoffeeTables.Add(table);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm bàn mới: {table.TableName} ({table.Area})";
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: /Admin/Table/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var table = await _context.CoffeeTables.FindAsync(id);
        if (table != null)
        {
            _context.CoffeeTables.Remove(table);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa bàn {table.TableName}";
        }
        return RedirectToAction(nameof(Index));
    }
}
