using System.ComponentModel.DataAnnotations;

namespace blazor_ecommerce_app.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
