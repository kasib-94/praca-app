using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{

    public class AuctionOffers
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int AuctionId { get; set; }

        public decimal? PriceInstant { get; set; }
        public decimal? PriceAuction { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Auction Auction { get; set; }

        public void Configure(EntityTypeBuilder<AuctionOffers> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
            .WithMany(x => x.AuctionOffers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Auction)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.AuctionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
