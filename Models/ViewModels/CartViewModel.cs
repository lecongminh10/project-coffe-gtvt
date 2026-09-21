using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models.ViewModels;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice => Price * Quantity;
}

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
    public int TotalQuantity => Items.Sum(x => x.Quantity);

    [Required(ErrorMessage = "Vui lòng nhập họ tên của bạn")]
    [Display(Name = "Họ và tên")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0")]
    [Display(Name = "Số điện thoại")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Display(Name = "Chọn bàn (nếu uống tại quán)")]
    public int? TableId { get; set; }

    [Display(Name = "Hình thức phục vụ")]
    public string OrderType { get; set; } = "DineIn"; // DineIn (Tại bàn), TakeAway (Mang về)

    [Display(Name = "Phương thức thanh toán")]
    public string PaymentMethod { get; set; } = "Tiền mặt";

    [Display(Name = "Ghi chú thêm (ít đường, nhiều đá, v.v.)")]
    public string? Notes { get; set; }
}
