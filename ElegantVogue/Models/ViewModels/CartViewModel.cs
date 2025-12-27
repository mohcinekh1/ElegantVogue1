namespace ElegantVogue.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; } = 10.00m;
        public decimal Total => Subtotal + Shipping;
        public int ItemCount => Items.Sum(i => i.Quantity);
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHex { get; set; }
        public string? SizeName { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}