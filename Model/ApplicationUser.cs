using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
