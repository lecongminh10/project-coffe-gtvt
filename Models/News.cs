using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

[Table("News")]
public class News
{
    [Key]
    public int NewsId { get; set; }

    [Required(ErrorMessage = "Tiêu đề bài viết không được để trống")]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [StringLength(255)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Summary { get; set; }

    public string Content { get; set; } = string.Empty;

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    public int? AuthorId { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.Now;

    public int ViewCount { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    // Navigation property
    [ForeignKey("AuthorId")]
    public virtual User? Author { get; set; }
}
