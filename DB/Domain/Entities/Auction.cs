using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public enum AuctionType
    {
        Instant = 1,
        Days_14 = 2,
        Days_7 = 3
    }
    public class Auction
    {
        public int Id { get; set; }
        public string Description { get; set; }


        public ICollection<AuctionAttachment> Attachments { get; set; }
        public ICollection<AuctionStatus> Status { get; set; }


        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
