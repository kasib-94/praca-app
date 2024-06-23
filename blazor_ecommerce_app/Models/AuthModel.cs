namespace blazor_ecommerce_app.Models
{
    public class AuthModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DB.SD.Roles Rola { get; set; }
        public string Token { get; set; }
    }
}
