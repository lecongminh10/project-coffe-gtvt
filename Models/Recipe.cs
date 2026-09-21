using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Recipes")]
public class Recipe
{
    [Key]
    public int RecipeId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int IngredientId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountNeeded { get; set; } = 0; // Định lượng cần dùng cho 1 ly

    [StringLength(30)]
    public string Unit { get; set; } = "g"; // g, ml, túi...

    // Navigation properties
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("IngredientId")]
    public virtual Ingredient? Ingredient { get; set; }
}
