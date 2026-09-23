using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("EmployeeSchedules")]
public class EmployeeSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ShiftId { get; set; }

    [Required]
    public DateTime WorkDate { get; set; } // Ngày làm việc

    [StringLength(50)]
    public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Absent

    [StringLength(255)]
    public string? Note { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    [NotMapped]
    public decimal WorkedHours => CheckInTime.HasValue && CheckOutTime.HasValue
        ? Math.Round((decimal)(CheckOutTime.Value - CheckInTime.Value).TotalHours, 2)
        : 0;

    [NotMapped]
    public decimal EstimatedWage => WorkedHours * (Shift?.HourlyWage ?? 0);

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    [ForeignKey("ShiftId")]
    public virtual Shift? Shift { get; set; }
}
