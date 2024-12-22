namespace ProductShipping.Models
{
    public class Package
    {
        public List<Product> Items { get; set; } = new List<Product>();
        public int TotalWeight { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CourierPrice { get; set; }
    }
}
