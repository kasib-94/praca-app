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

        public ICollection<Auction> Auctions { get; set; }
        public ICollection<AuctionOffers> AuctionOffers { get; set; }

        public ICollection<Auction> BoughtAuctions { get; set; }




        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Auctions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.BoughtAuctions)
           .WithOne(x => x.Buyer)
           .HasForeignKey(x => x.BuyerId);
        }
    }
}
