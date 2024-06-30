namespace DB.Models
{
    public class NewAuction
    {
        public decimal PriceAuction { get; set; }
        public decimal PriceInstant { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DB.Domain.Entities.AuctionType AuctionType { get; set; }

        public List<Photo> Photos { get; set; } = new();

        public List<AuctionStatus> Statuses { get; set; }

        public bool EnableInstant
        {
            get
            {
                return SD.AuctionSD.IsInstantBuy(AuctionType);
            }
            set { }
        }

        public bool EnableAuction
        {
            get
            {
                return SD.AuctionSD.IsInstantBuy(AuctionType);
            }
            set { }
        }

    }
    public class AuctionStatus
    {
        public DB.Domain.Entities.AuctionStatusType Type { get; set; }
        public DateTime ActionDate { get; set; }
        public int AuctionId { get; set; }
    }

    public class Offer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public string UserName { get; set; } = "";
        public decimal? PriceInstant { get; set; }
        public decimal? PriceAuction { get; set; }
        public DateTime Date { get; set; }
    }

    public class Photo
    {
        public byte[] ImageData { get; set; }
        public int Order { get; set; }
        public bool Minature { get; set; } = false;
        public string Extension { get; set; } = "";
        public string UniqueId { get; set; }


    }
}
