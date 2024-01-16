namespace BigBasketApplication.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public double GstPercentage { get; set; } 
        public int DiscountPercentage { get; set; }
    }
}
