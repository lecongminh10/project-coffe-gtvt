using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Vouchers")]
public class Voucher
{
    [Key]
    public int VoucherId { get; set; }

    [Required(ErrorMessage = "Mã voucher không được để trống")]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "% giảm giá từ 1 đến 100")]
    public int DiscountPercent { get; set; } = 10;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaxDiscountAmount { get; set; } = 50000;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinOrderAmount { get; set; } = 0;

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);

    public int UsageLimit { get; set; } = 100;

    public int UsedCount { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}
