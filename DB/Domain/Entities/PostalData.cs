using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public class PostalData
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int AuctionId { get; set; }

        public string Adress { get; set; }
        public string PostCode { get; set; }

        public string PhoneNumber { get; set; }

        public bool Confirmed { get; set; } = false;
        public User User { get; set; }
        public Auction Auction { get; set; }


        public void Configure(EntityTypeBuilder<PostalData> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Auction)
                .WithMany()
                .HasForeignKey(x => x.AuctionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
