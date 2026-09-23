using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models.ViewModels;

public class InventoryViewModel
{
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Supplier> Suppliers { get; set; } = new();
    public List<StockTransaction> RecentTransactions { get; set; } = new();
}

public class RecipeManagementViewModel
{
    public List<Recipe> Recipes { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public List<Ingredient> Ingredients { get; set; } = new();
}

public class WorkforceViewModel
{
    public List<EmployeeSchedule> Schedules { get; set; } = new();
    public List<User> Staff { get; set; } = new();
    public List<Shift> Shifts { get; set; } = new();
    public decimal TotalEstimatedWage => Schedules.Sum(x => x.EstimatedWage);
}

public class AdminUserFormViewModel
{
    public int UserId { get; set; }

    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0")]
    public string? PhoneNumber { get; set; }

    [Required]
    public string Role { get; set; } = "Staff";

    [DataType(DataType.Password), MinLength(6)]
    public string? Password { get; set; }
}
