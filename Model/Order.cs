namespace BigBasketApplication.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime DelieverySlot { get; set; }
        public double TotalAmount { get; set; }
    }
}
