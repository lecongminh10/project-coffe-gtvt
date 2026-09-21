using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Reviews")]
public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    public int ProductId { get; set; }

    public int? UserId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Đánh giá từ 1 đến 5 sao")]
    public int Rating { get; set; } = 5;

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsApproved { get; set; } = true;

    // Navigation properties
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
