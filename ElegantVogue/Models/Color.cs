namespace ElegantVogue.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    }
}