namespace ECommerceApp.Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}