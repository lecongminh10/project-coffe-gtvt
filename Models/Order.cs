using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Display(Name = "Thời gian đặt")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    [StringLength(100)]
    [Display(Name = "Tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? CustomerPhone { get; set; }

    [Display(Name = "Bàn phục vụ")]
    public int? TableId { get; set; }

    [ForeignKey("TableId")]
    public virtual CoffeeTable? Table { get; set; }

    [Display(Name = "Tài khoản")]
    public int? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền (VNĐ)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Trạng thái đơn")]
    public string Status { get; set; } = "Pending"; // Pending (Chờ xử lý), Processing (Đang pha chế), Completed (Hoàn thành), Cancelled (Đã hủy)

    [StringLength(50)]
    [Display(Name = "Phương thức thanh toán")]
    public string PaymentMethod { get; set; } = "Tiền mặt"; // Tiền mặt, Chuyển khoản, MoMo, Thẻ

    [Display(Name = "Trạng thái thanh toán")]
    public bool IsPaid { get; set; } = false;

    [StringLength(500)]
    [Display(Name = "Ghi chú đơn hàng")]
    public string? Notes { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
