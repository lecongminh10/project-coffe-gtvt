namespace BaiTapLon.Models.ViewModels;

public class ProductFilterViewModel
{
    public List<Product> Products { get; set; } = new List<Product>();
    public List<Category> Categories { get; set; } = new List<Category>();

    public int? SelectedCategoryId { get; set; }
    public string? SearchKeyword { get; set; }
    public string? SortOrder { get; set; } // price_asc, price_desc, name_asc

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
