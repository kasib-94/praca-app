using DB.Domain;
using DB.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Queries
{
    public class GetAuctionById
    {
        public class Request : IRequest<Response>
        {
            public int AuctionId { get; set; }

        }

        public class Response
        {
            public decimal? PriceAuction { get; set; }
            public decimal? PriceInstant { get; set; }
            public decimal? PriceAuctionStart { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DB.Domain.Entities.AuctionType AuctionType { get; set; }

            public DateTime AuctionStarted { get; set; }

            public DateTime AuctionFinish { get; set; }

            public List<Photo> Photos { get; set; } = new();

            public List<AuctionStatus> Statuses { get; set; } = new();


            public List<Offer> Offers { get; set; } = new();
        }

        private class LoginHandler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext _dbContext;
            public LoginHandler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return (await _dbContext.Auctions
                     .Where(x => x.Id == request.AuctionId)
                     .Select(x => new Response()
                     {
                         AuctionFinish = x.AuctionFinish,
                         AuctionStarted = x.AuctionStart,

                         PriceAuctionStart = x.IsAuction ? x.PriceAuctionStart : null,
                         PriceAuction = x.IsAuction
                           ? (x.Offers.Any(z => z.PriceAuction.HasValue == true)
                                        ? x.Offers.Max(x => x.PriceAuction) : null)
                           : null,
                         PriceInstant = x.IsInstantBuy
                           ? (x.Offers.Any(z => z.PriceInstant.HasValue == true)
                                        ? x.Offers.Max(x => x.PriceInstant) : null)
                           : null,
                         Title = x.Title,
                         Description = x.Description,

                         AuctionType = x.Type,
                         Photos = x.Attachments.Select(y => new Photo()
                         {
                             Extension = y.Extension,
                             ImageData = y.ImageData,
                             Minature = y.Miniature,
                             Order = y.Order,

                         }).ToList(),

                         Statuses = x.Status.Select(y => new AuctionStatus()
                         {
                             ActionDate = y.ActionDate,
                             AuctionId = y.AuctionId,
                             Type = y.Type,
                         }).ToList(),
                         Offers = x.Offers.Select(y => new Offer()
                         {
                             AuctionId = y.AuctionId,
                             Date = y.Date,
                             Id = y.Id,
                             PriceAuction = y.PriceAuction,
                             PriceInstant = y.PriceInstant

                         }).ToList()




                     }).ToListAsync()).First();


            }
        }


    }
}
