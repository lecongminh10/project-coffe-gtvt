using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Suppliers")]
public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ContactPerson { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    // Navigation property
    public virtual ICollection<Ingredient>? Ingredients { get; set; }
}
