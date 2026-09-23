using BaiTapLon.Data;
using BaiTapLon.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Services;

public record InventoryResult(bool Success, string Message)
{
    public static InventoryResult Ok(string message) => new(true, message);
    public static InventoryResult Fail(string message) => new(false, message);
}

public class InventoryService
{
    private readonly CoffeeShopDbContext _context;

    public InventoryService(CoffeeShopDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryResult> DeductForOrderAsync(Order order)
    {
        if (order.InventoryDeducted)
            return InventoryResult.Ok("Đơn hàng đã được trừ kho trước đó.");

        var requirements = await BuildRequirementsAsync(order);
        foreach (var requirement in requirements)
        {
            if (requirement.Ingredient.QuantityInStock < requirement.StockQuantity)
            {
                return InventoryResult.Fail(
                    $"Không đủ {requirement.Ingredient.Name}. Cần {requirement.StockQuantity:N2} " +
                    $"{requirement.Ingredient.Unit}, hiện còn {requirement.Ingredient.QuantityInStock:N2} {requirement.Ingredient.Unit}.");
            }
        }

        foreach (var requirement in requirements)
        {
            requirement.Ingredient.QuantityInStock -= requirement.StockQuantity;
            _context.StockTransactions.Add(new StockTransaction
            {
                IngredientId = requirement.Ingredient.IngredientId,
                OrderId = order.OrderId,
                TransactionType = "OrderOut",
                Quantity = -requirement.StockQuantity,
                BalanceAfter = requirement.Ingredient.QuantityInStock,
                Note = $"Xuất kho cho đơn #{order.OrderId}"
            });
        }

        order.InventoryDeducted = true;
        return InventoryResult.Ok($"Đã trừ kho {requirements.Count} nguyên liệu theo định lượng.");
    }

    public async Task<InventoryResult> RestoreForOrderAsync(Order order)
    {
        if (!order.InventoryDeducted)
            return InventoryResult.Ok("Đơn hàng chưa trừ kho nên không cần hoàn kho.");

        var requirements = await BuildRequirementsAsync(order);
        foreach (var requirement in requirements)
        {
            requirement.Ingredient.QuantityInStock += requirement.StockQuantity;
            _context.StockTransactions.Add(new StockTransaction
            {
                IngredientId = requirement.Ingredient.IngredientId,
                OrderId = order.OrderId,
                TransactionType = "OrderReturn",
                Quantity = requirement.StockQuantity,
                BalanceAfter = requirement.Ingredient.QuantityInStock,
                Note = $"Hoàn kho do hủy đơn #{order.OrderId}"
            });
        }

        order.InventoryDeducted = false;
        return InventoryResult.Ok($"Đã hoàn kho {requirements.Count} nguyên liệu.");
    }

    private async Task<List<IngredientRequirement>> BuildRequirementsAsync(Order order)
    {
        if (order.OrderDetails.Count == 0)
        {
            await _context.Entry(order)
                .Collection(o => o.OrderDetails)
                .LoadAsync();
        }

        var productIds = order.OrderDetails.Select(x => x.ProductId).Distinct().ToList();
        var recipes = await _context.Recipes
            .Include(r => r.Ingredient)
            .Where(r => productIds.Contains(r.ProductId))
            .ToListAsync();

        var quantities = order.OrderDetails
            .GroupBy(x => x.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

        var result = new List<IngredientRequirement>();
        foreach (var group in recipes.GroupBy(r => r.IngredientId))
        {
            var ingredient = group.First().Ingredient
                ?? throw new InvalidOperationException("Công thức không có nguyên liệu hợp lệ.");
            if (ingredient.BaseUnitsPerStockUnit <= 0)
                throw new InvalidOperationException($"Hệ số quy đổi của {ingredient.Name} phải lớn hơn 0.");

            decimal baseQuantity = 0;
            foreach (var recipe in group)
            {
                if (!string.Equals(recipe.Unit, ingredient.BaseUnit, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Đơn vị công thức {recipe.Unit} không khớp đơn vị cơ sở {ingredient.BaseUnit} của {ingredient.Name}.");
                }

                baseQuantity += recipe.AmountNeeded * quantities.GetValueOrDefault(recipe.ProductId);
            }

            result.Add(new IngredientRequirement(
                ingredient,
                Math.Round(baseQuantity / ingredient.BaseUnitsPerStockUnit, 4, MidpointRounding.AwayFromZero)));
        }

        return result;
    }

    private record IngredientRequirement(Ingredient Ingredient, decimal StockQuantity);
}
