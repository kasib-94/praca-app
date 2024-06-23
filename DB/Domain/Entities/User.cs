using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public SD.Roles Role { get; set; }



        public void Configure(EntityTypeBuilder<User> builder)
        {


        }
    }
}
