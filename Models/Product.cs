using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Tên món không được để trống")]
    [StringLength(150, ErrorMessage = "Tên món không quá 150 ký tự")]
    [Display(Name = "Tên món / Đồ uống")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
    [Range(1000, 10000000, ErrorMessage = "Đơn giá phải từ 1.000đ đến 10.000.000đ")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Đơn giá (VNĐ)")]
    public decimal Price { get; set; }

    [Display(Name = "Mô tả chi tiết")]
    [StringLength(1000, ErrorMessage = "Mô tả không vượt quá 1000 ký tự")]
    public string? Description { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Còn phục vụ")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Món bán chạy (Hot)")]
    public bool IsFeatured { get; set; } = false;

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
