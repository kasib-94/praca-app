using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public enum StripeStatus
    {
        Pending = 1,
        Failed = 2,
        Success = 3
    }

    public class StripeSession
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string SessionUrl { get; set; }

        public int AuctionId { get; set; }

        public StripeStatus StripeStatus { get; set; } = StripeStatus.Pending;

        public Auction Auction { get; set; }

        public void Configure(EntityTypeBuilder<StripeSession> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Auction)
                .WithMany()
                .HasForeignKey(x => x.AuctionId);


        }
    }
}
