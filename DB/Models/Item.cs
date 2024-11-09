using DB.Domain.Entities;

namespace DB.Models
{
    public interface IItem
    {

        public string Title { get; set; }

        public AuctionType Type { get; set; }

        public byte[]? Minature { get; set; }

        public string? Extension { get; set; }
    }
}
