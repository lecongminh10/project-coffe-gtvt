using BaiTapLon.Data;
using BaiTapLon.Models;
using BaiTapLon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class RecipeController : Controller
{
    private readonly CoffeeShopDbContext _context;
    public RecipeController(CoffeeShopDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(new RecipeManagementViewModel
    {
        Recipes = await _context.Recipes.Include(r => r.Product).Include(r => r.Ingredient)
            .OrderBy(r => r.Product!.Name).ToListAsync(),
        Products = await _context.Products.OrderBy(p => p.Name).ToListAsync(),
        Ingredients = await _context.Ingredients.OrderBy(i => i.Name).ToListAsync()
    });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Recipe recipe)
    {
        var ingredient = await _context.Ingredients.FindAsync(recipe.IngredientId);
        if (ingredient == null || recipe.AmountNeeded <= 0)
        {
            TempData["ErrorMessage"] = "Công thức hoặc nguyên liệu không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }
        recipe.Unit = ingredient.BaseUnit;
        var duplicate = await _context.Recipes.AnyAsync(r => r.ProductId == recipe.ProductId &&
            r.IngredientId == recipe.IngredientId && r.RecipeId != recipe.RecipeId);
        if (duplicate)
        {
            TempData["ErrorMessage"] = "Nguyên liệu này đã có trong công thức của món.";
            return RedirectToAction(nameof(Index));
        }
        if (recipe.RecipeId == 0) _context.Recipes.Add(recipe);
        else
        {
            var existing = await _context.Recipes.FindAsync(recipe.RecipeId);
            if (existing == null) return NotFound();
            existing.ProductId = recipe.ProductId;
            existing.IngredientId = recipe.IngredientId;
            existing.AmountNeeded = recipe.AmountNeeded;
            existing.Unit = recipe.Unit;
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã lưu định lượng công thức.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe != null) { _context.Recipes.Remove(recipe); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
