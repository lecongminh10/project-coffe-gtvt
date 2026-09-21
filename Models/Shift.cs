using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("Shifts")]
public class Shift
{
    [Key]
    public int ShiftId { get; set; }

    [Required(ErrorMessage = "Tên ca không được để trống")]
    [StringLength(50)]
    public string ShiftName { get; set; } = string.Empty; // Ca sáng, Ca chiều, Ca tối

    [Required]
    public TimeSpan StartTime { get; set; } // 07:00:00

    [Required]
    public TimeSpan EndTime { get; set; } // 12:00:00

    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyWage { get; set; } = 25000; // Lương mỗi giờ

    // Navigation property
    public virtual ICollection<EmployeeSchedule>? EmployeeSchedules { get; set; }
}
