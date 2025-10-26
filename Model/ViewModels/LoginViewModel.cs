using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Model.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui long nhap email")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui long nhap mat khau")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ghi nho dang nhap")]
        public bool RememberMe { get; set; }
    }
}
