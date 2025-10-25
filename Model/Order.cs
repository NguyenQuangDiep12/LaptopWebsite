using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ECommerceApp.Model
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}