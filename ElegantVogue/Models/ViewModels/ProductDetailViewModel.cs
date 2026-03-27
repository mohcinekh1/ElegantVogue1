namespace ElegantVogue.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; } = null!;
        public List<Color> AvailableColors { get; set; } = new List<Color>();
        public List<Size> AvailableSizes { get; set; } = new List<Size>();
        public int? SelectedColorId { get; set; }
        public int? SelectedSizeId { get; set; }
    }
}