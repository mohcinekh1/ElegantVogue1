namespace ElegantVogue.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductType { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsNewThisWeek { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public int? CollectionId { get; set; }

        public Category? Category { get; set; }
        public Collection? Collection { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}