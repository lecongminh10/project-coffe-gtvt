using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;
using BaiTapLon.Models.ViewModels;

namespace BaiTapLon.Controllers;

public class ProductController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public ProductController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: /Product
    public async Task<IActionResult> Index(int? categoryId, string? search, int page = 1, string? sort = null)
    {
        int pageSize = 6;
        var query = _context.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        // Lọc theo danh mục (Bài TH 5)
        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Tìm kiếm theo từ khóa (Bài TH 5)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(keyword) || 
                                     (p.Description != null && p.Description.ToLower().Contains(keyword)));
        }

        // Sắp xếp
        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            _ => query.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.ProductId)
        };

        int totalItems = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();

        var viewModel = new ProductFilterViewModel
        {
            Products = products,
            Categories = categories,
            SelectedCategoryId = categoryId,
            SearchKeyword = search,
            SortOrder = sort,
            PageIndex = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };

        return View(viewModel);
    }

    // AJAX Action: Trả về PartialView khi lọc hoặc tìm kiếm hoặc phân trang (Bài TH 5)
    [HttpGet]
    public async Task<IActionResult> GetProductsPartial(int? categoryId, string? search, int page = 1, string? sort = null)
    {
        int pageSize = 6;
        var query = _context.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(keyword) || 
                                     (p.Description != null && p.Description.ToLower().Contains(keyword)));
        }

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            _ => query.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.ProductId)
        };

        int totalItems = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new ProductFilterViewModel
        {
            Products = products,
            SelectedCategoryId = categoryId,
            SearchKeyword = search,
            SortOrder = sort,
            PageIndex = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };

        return PartialView("_ProductListPartial", viewModel);
    }

    // GET: /Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.ProductId == id);

        if (product == null) return NotFound();

        // Lấy thêm 4 món liên quan cùng category
        ViewBag.RelatedProducts = await _context.Products
            .Where(p => p.CategoryId == product.CategoryId && p.ProductId != product.ProductId && p.IsAvailable)
            .OrderByDescending(p => p.ProductId)
            .Take(4)
            .ToListAsync();

        return View(product);
    }
}
