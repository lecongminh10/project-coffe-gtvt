using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    [StringLength(100, ErrorMessage = "Tên danh mục không vượt quá 100 ký tự")]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Mô tả không vượt quá 255 ký tự")]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Icon / Hình ảnh")]
    public string? Icon { get; set; }

    [Display(Name = "Thứ tự hiển thị")]
    public int DisplayOrder { get; set; } = 0;

    // Navigation property
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
