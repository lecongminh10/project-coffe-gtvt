using Microsoft.AspNetCore.Mvc;
using BaiTapLon.Helpers;
using BaiTapLon.Models.ViewModels;

namespace BaiTapLon.ViewComponents;

public class CartWidgetViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        int totalCount = cart.Sum(c => c.Quantity);
        decimal totalAmount = cart.Sum(c => c.TotalPrice);

        ViewBag.TotalCount = totalCount;
        ViewBag.TotalAmount = totalAmount;

        return View("Default", totalCount);
    }
}
