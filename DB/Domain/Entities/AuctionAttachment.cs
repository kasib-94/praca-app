using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public class AuctionAttachment
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public int Order { get; set; }
        public int AuctionId { get; set; }

        public Auction Auction { get; set; }


        public void Configure(EntityTypeBuilder<AuctionAttachment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Auction)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.AuctionId);

        }
    }
}
