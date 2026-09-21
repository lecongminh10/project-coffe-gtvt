using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models;

public class CoffeeTable
{
    [Key]
    public int TableId { get; set; }

    [Required(ErrorMessage = "Số bàn/Tên bàn không được để trống")]
    [StringLength(50, ErrorMessage = "Tên bàn không quá 50 ký tự")]
    [Display(Name = "Tên bàn")]
    public string TableName { get; set; } = string.Empty;

    [Range(1, 30, ErrorMessage = "Sức chứa từ 1 đến 30 người")]
    [Display(Name = "Sức chứa (Người)")]
    public int Capacity { get; set; } = 4;

    [Display(Name = "Khu vực")]
    [StringLength(50)]
    public string Area { get; set; } = "Tầng 1"; // Tầng 1, Tầng 2, Ngoài trời, Phòng VIP

    [Display(Name = "Trạng thái bàn")]
    [StringLength(30)]
    public string Status { get; set; } = "Available"; // Available (Trống), Occupied (Có khách), Reserved (Đã đặt)

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
