using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public class AuctionAttachment
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }

        public int Order { get; set; }
        public int AuctionId { get; set; }
        public bool Miniature { get; set; } = false;
        public string Extension { get; set; }
        public Auction Auction { get; set; }


        public void Configure(EntityTypeBuilder<AuctionAttachment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Auction)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.AuctionId)
                                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
