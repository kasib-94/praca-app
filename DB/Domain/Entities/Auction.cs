using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.Domain.Entities
{
    public enum AuctionType
    {
        [Display(Name = "Instant buy")]
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
        [ForeignKey("UserId")]
        public User User { get; set; }

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public AuctionType Type { get; set; }

        public string AuctionGuid { get; set; } = "";

        public decimal PriceInstant { get; set; }
        public decimal PriceAuctionStart { get; set; }
        public int? BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public User? Buyer { get; set; }

        public DateTime AuctionFinish { get; set; }
        public DateTime AuctionStart { get; set; }

        [NotMapped]
        public bool IsInstantBuy
        {
            get
            {
                return DB.SD.AuctionSD.IsInstantBuy(Type);
            }
        }

        [NotMapped]
        public bool IsAuction
        {
            get
            {
                return DB.SD.AuctionSD.IsAuction(Type);
            }
        }

        public ICollection<AuctionAttachment> Attachments { get; set; }
        public ICollection<AuctionStatus> Status { get; set; }
        public ICollection<AuctionOffers> Offers { get; set; }
    }

    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(x => x.Id);



            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.AuctionGuid)
                .IsRequired()
                .HasMaxLength(36);
        }
    }
}
