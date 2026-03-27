namespace ElegantVogue.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Product? Product { get; set; }
        public Color? Color { get; set; }
        public Size? Size { get; set; }
    }
}