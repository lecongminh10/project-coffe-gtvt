using BaiTapLon.Data;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class InventoryController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public InventoryController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        return View(new InventoryViewModel
        {
            Ingredients = await _context.Ingredients.Include(i => i.Supplier).OrderBy(i => i.Name).ToListAsync(),
            Suppliers = await _context.Suppliers.OrderBy(s => s.Name).ToListAsync(),
            RecentTransactions = await _context.StockTransactions.Include(t => t.Ingredient)
                .OrderByDescending(t => t.CreatedAt).Take(30).ToListAsync()
        });
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateIngredient(Ingredient ingredient)
    {
        if (ingredient.BaseUnitsPerStockUnit <= 0) ModelState.AddModelError("", "Hệ số quy đổi phải lớn hơn 0.");
        if (!ModelState.IsValid) return await InvalidIndex("Thông tin nguyên liệu không hợp lệ.");
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Đã thêm nguyên liệu {ingredient.Name}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditIngredient(Ingredient ingredient)
    {
        if (ingredient.BaseUnitsPerStockUnit <= 0) ModelState.AddModelError("", "Hệ số quy đổi phải lớn hơn 0.");
        if (!ModelState.IsValid) return await InvalidIndex("Thông tin nguyên liệu không hợp lệ.");
        var existing = await _context.Ingredients.FindAsync(ingredient.IngredientId);
        if (existing == null) return NotFound();
        existing.Name = ingredient.Name;
        existing.Unit = ingredient.Unit;
        existing.BaseUnit = ingredient.BaseUnit;
        existing.BaseUnitsPerStockUnit = ingredient.BaseUnitsPerStockUnit;
        existing.MinimumStock = ingredient.MinimumStock;
        existing.UnitPrice = ingredient.UnitPrice;
        existing.SupplierId = ingredient.SupplierId;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Đã cập nhật {existing.Name}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AdjustStock(int ingredientId, decimal quantity, string? note)
    {
        var ingredient = await _context.Ingredients.FindAsync(ingredientId);
        if (ingredient == null) return NotFound();
        if (quantity == 0 || ingredient.QuantityInStock + quantity < 0)
        {
            TempData["ErrorMessage"] = "Số lượng điều chỉnh không hợp lệ hoặc làm tồn kho âm.";
            return RedirectToAction(nameof(Index));
        }
        ingredient.QuantityInStock += quantity;
        _context.StockTransactions.Add(new StockTransaction
        {
            IngredientId = ingredientId,
            TransactionType = quantity > 0 ? "StockIn" : "AdjustmentOut",
            Quantity = quantity,
            BalanceAfter = ingredient.QuantityInStock,
            Note = string.IsNullOrWhiteSpace(note) ? "Điều chỉnh tồn kho thủ công" : note.Trim()
        });
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Tồn kho {ingredient.Name}: {ingredient.QuantityInStock:N2} {ingredient.Unit}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var ingredient = await _context.Ingredients.Include(i => i.Recipes).FirstOrDefaultAsync(i => i.IngredientId == id);
        if (ingredient == null) return NotFound();
        if (ingredient.Recipes.Count > 0)
        {
            TempData["ErrorMessage"] = "Không thể xóa nguyên liệu đang được dùng trong công thức.";
            return RedirectToAction(nameof(Index));
        }
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã xóa nguyên liệu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveSupplier(Supplier supplier)
    {
        if (!ModelState.IsValid) return await InvalidIndex("Thông tin nhà cung cấp không hợp lệ.");
        if (supplier.SupplierId == 0) _context.Suppliers.Add(supplier);
        else
        {
            var existing = await _context.Suppliers.FindAsync(supplier.SupplierId);
            if (existing == null) return NotFound();
            existing.Name = supplier.Name;
            existing.ContactPerson = supplier.ContactPerson;
            existing.PhoneNumber = supplier.PhoneNumber;
            existing.Email = supplier.Email;
            existing.Address = supplier.Address;
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu nhà cung cấp.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.Include(s => s.Ingredients).FirstOrDefaultAsync(s => s.SupplierId == id);
        if (supplier == null) return NotFound();
        if (supplier.Ingredients?.Count > 0)
        {
            TempData["ErrorMessage"] = "Không thể xóa nhà cung cấp đang có nguyên liệu.";
            return RedirectToAction(nameof(Index));
        }
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> InvalidIndex(string message)
    {
        TempData["ErrorMessage"] = message;
        return RedirectToAction(nameof(Index));
    }
}
