namespace ElegantVogue.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public List<Product> NewThisWeek { get; set; } = new List<Product>();
        public List<Product> CollectionProducts { get; set; } = new List<Product>();
        public Collection? CurrentCollection { get; set; }
    }
}