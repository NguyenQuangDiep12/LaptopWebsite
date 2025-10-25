using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ten san pham la bat buoc")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Gia phai tra lon hon 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}