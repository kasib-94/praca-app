using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public enum AuctionType
    {
        [Display(Name = "Auction")]
        Auction = 0,
        [Display(Name = "Instant auction")]
        Instant = 1,
        [Display(Name = "14 days auction")]
        Days_14 = 2,
        [Display(Name = "7 days auction")]
        Days_7 = 3,
        [Display(Name = "7 days auction with instant buy")]
        Days_7_With_Instant = 4,
        [Display(Name = "14 days auction with instant buy")]
        Days_14_With_Instant = 5
    }
    public class Auction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public AuctionType Type { get; set; }

        public string AuctionGuid { get; set; } = "";

        public decimal PriceInstant { get; set; }
        public decimal PriceAuctionStart { get; set; }


        public DateTime AuctionFinish { get; set; }
        public DateTime AuctionStart { get; set; }

        [NotMapped]
        public bool IsInstantBuy
        {
            get
            {
                return Type == AuctionType.Instant ||
                    Type == AuctionType.Days_7_With_Instant ||
                    Type == AuctionType.Days_14_With_Instant;
            }
        }

        [NotMapped]
        public bool IsAuction
        {
            get
            {
                return Type == AuctionType.Auction ||
                    Type == AuctionType.Days_7_With_Instant ||
                    Type == AuctionType.Days_14_With_Instant ||
                    Type == AuctionType.Days_14 ||
                    Type == AuctionType.Days_7;
            }
        }

        public User User { get; set; }
        public ICollection<AuctionAttachment> Attachments { get; set; }
        public ICollection<AuctionStatus> Status { get; set; }
        public ICollection<AuctionOffers> Offers { get; set; }


        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
          .WithMany(x => x.Auctions)
          .HasForeignKey(x => x.UserId)
          .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
