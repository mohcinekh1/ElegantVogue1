namespace ElegantVogue.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "Pending";

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? StateRegion { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}