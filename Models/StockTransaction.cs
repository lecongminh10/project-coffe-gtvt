using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models;

public class StockTransaction
{
    [Key]
    public int StockTransactionId { get; set; }

    [Required]
    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }

    [Required, StringLength(20)]
    public string TransactionType { get; set; } = "Adjustment";

    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal BalanceAfter { get; set; }

    public int? OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
