using DB.Domain.Entities;

namespace DB.Models
{
    public enum TypKomponentuItem
    {
        Brak = 0,
        DoZaplaty = 1,
        TwojeAukcje = 2,
        AukcjeZakonczone = 3,
        Oplacone = 4
    }

    public interface IItem
    {
        public int? AuctionId { get; set; }
        public string Title { get; set; }

        public AuctionType Type { get; set; }

        public byte[]? Minature { get; set; }

        public string? Extension { get; set; }
        public int? OwnerId { get; set; }
        public string OwnerName { get; set; }

        public DB.Domain.Entities.AuctionStatusType Status { get; set; }


    }
}
