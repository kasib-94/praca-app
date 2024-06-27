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

        public bool EnableInstant
        {
            get
            {
                return AuctionType == DB.Domain.Entities.AuctionType.Instant ||
                          AuctionType == DB.Domain.Entities.AuctionType.Days_7_With_Instant ||
                          AuctionType == DB.Domain.Entities.AuctionType.Days_14_With_Instant;
            }
            set { }
        }

        public bool EnableAuction
        {
            get
            {
                return AuctionType == DB.Domain.Entities.AuctionType.Auction ||
                          AuctionType == DB.Domain.Entities.AuctionType.Days_7_With_Instant ||
                          AuctionType == DB.Domain.Entities.AuctionType.Days_14_With_Instant;
            }
            set { }
        }

    }

    public class Photo
    {
        public string Path { get; set; } = "";
        public int Order { get; set; }
        public bool Minature { get; set; } = false;
        public string Extension { get; set; }

        public MemoryStream Stream { get; set; }

    }
}
