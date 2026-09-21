using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public CategoryController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: /Admin/Category
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return View(categories);
    }

    // GET: /Admin/Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm danh mục \"{category.Name}\" thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: /Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    // POST: /Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.CategoryId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Cập nhật danh mục \"{category.Name}\" thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.CategoryId == category.CategoryId))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // POST: /Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null) return NotFound();

        if (category.Products.Any())
        {
            TempData["ErrorMessage"] = $"Không thể xóa danh mục \"{category.Name}\" vì đang có {category.Products.Count} món thuộc danh mục này.";
            return RedirectToAction(nameof(Index));
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Đã xóa danh mục \"{category.Name}\" thành công!";
        return RedirectToAction(nameof(Index));
    }
}
