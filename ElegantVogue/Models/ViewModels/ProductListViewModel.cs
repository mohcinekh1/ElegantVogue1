namespace ElegantVogue.Models.ViewModels
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Color> Colors { get; set; } = new List<Color>();
        public List<Size> Sizes { get; set; } = new List<Size>();
        public List<Collection> Collections { get; set; } = new List<Collection>();

        public string? SearchQuery { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedCollectionId { get; set; }
        public List<int> SelectedSizeIds { get; set; } = new List<int>();
        public List<int> SelectedColorIds { get; set; } = new List<int>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStockOnly { get; set; }
        public string? SortBy { get; set; }

        public int TotalProducts { get; set; }
        public int AvailableCount { get; set; }
        public int OutOfStockCount { get; set; }
    }
}