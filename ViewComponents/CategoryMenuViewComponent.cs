using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;

namespace BaiTapLon.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly CoffeeShopDbContext _context;

    public CategoryMenuViewComponent(CoffeeShopDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? selectedCategoryId = null)
    {
        ViewBag.SelectedCategoryId = selectedCategoryId;
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return View("Default", categories);
    }
}
