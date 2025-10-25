using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Model.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui long nhap ho ten")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui long nhap email")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui long nhap so dien thoai")]
        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui long nhap dia chi")]
        public string Address { get; set; }

        public List<CartItemViewModel> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}