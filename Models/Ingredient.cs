using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Ingredients")]
public class Ingredient
{
    [Key]
    public int IngredientId { get; set; }

    [Required(ErrorMessage = "Tên nguyên liệu không được để trống")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Unit { get; set; } = "kg"; // kg, lít, hộp, gói

    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityInStock { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumStock { get; set; } = 5;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;

    public int? SupplierId { get; set; }

    // Navigation properties
    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<Recipe>? Recipes { get; set; }
}
