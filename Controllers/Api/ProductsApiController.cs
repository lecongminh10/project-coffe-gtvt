using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;

namespace BaiTapLon.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly CoffeeShopDbContext _context;

    public ProductsApiController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: api/ProductsApi
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId)
    {
        var query = _context.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var products = await query
            .Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.ImageUrl,
                p.IsFeatured,
                Category = new
                {
                    p.Category!.CategoryId,
                    p.Category.Name
                }
            })
            .ToListAsync();

        return Ok(products);
    }

    // GET: api/ProductsApi/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.ProductId == id)
            .Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.ImageUrl,
                p.IsAvailable,
                p.IsFeatured,
                Category = new
                {
                    p.Category!.CategoryId,
                    p.Category.Name
                }
            })
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound(new { message = $"Không tìm thấy món với ID = {id}" });
        }

        return Ok(product);
    }

    // GET: api/ProductsApi/search?q=ca+phe
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống" });
        }

        var keyword = q.Trim().ToLower();
        var results = await _context.Products
            .Where(p => p.IsAvailable && (p.Name.ToLower().Contains(keyword) || (p.Description != null && p.Description.ToLower().Contains(keyword))))
            .Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Price,
                p.ImageUrl
            })
            .OrderBy(p => p.Name)
            .Take(10)
            .ToListAsync();

        return Ok(results);
    }
}
