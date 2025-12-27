namespace ElegantVogue.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Season { get; set; }
        public string? Year { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}