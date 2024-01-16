namespace BigBasketApplication.Model
{
    public class ProductForCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double GstPercentage { get; set; }
        public int DiscountPercentage { get; set; }
    }
}
