using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly CoffeeShopDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(CoffeeShopDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: /Admin/Product
    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        var products = await query.OrderByDescending(p => p.ProductId).ToListAsync();

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", categoryId);
        ViewBag.CurrentCategory = categoryId;
        ViewBag.CurrentSearch = search;

        return View(products);
    }

    // GET: /Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View();
    }

    // POST: /Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.ImageUrl = "/uploads/" + uniqueFileName;
            }
            else if (string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = "/images/products/ca-phe-den.jpg";
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm món \"{product.Name}\" thành công!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // GET: /Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // POST: /Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
    {
        if (id != product.ProductId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                if (existing == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrl = "/uploads/" + uniqueFileName;
                }
                else
                {
                    product.ImageUrl = existing.ImageUrl;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Cập nhật món \"{product.Name}\" thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == product.ProductId))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // POST: /Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa món \"{product.Name}\" thành công!";
        }
        return RedirectToAction(nameof(Index));
    }
}
