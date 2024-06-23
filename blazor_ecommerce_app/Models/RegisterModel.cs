using System.ComponentModel.DataAnnotations;

namespace blazor_ecommerce_app.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string PasswordRepeat { get; set; } = "";
    }
}
