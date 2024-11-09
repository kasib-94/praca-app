using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{

    public enum AuctionStatusType
    {
        Start = 1,
        Finished = 2,
        FinishedByOwner = 3,
        PaymentConfirmed = 4
    }



    public class AuctionStatus
    {
        public int Id { get; set; }
        public AuctionStatusType Type { get; set; }

        public DateTime ActionDate { get; set; }

        public int AuctionId { get; set; }
        public Auction Auction { get; set; }




        public void Configure(EntityTypeBuilder<AuctionStatus> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Auction)
                .WithMany(x => x.Status)
                .HasForeignKey(x => x.AuctionId)
                                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
