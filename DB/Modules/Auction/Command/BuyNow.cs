using DB.Domain;
using DB.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Command
{
    public class BuyNow
    {
        public class Request : IRequest
        {
            public int UserId { get; set; }
            public int AuctionId { get; set; }

        }
        private class Handler : IRequestHandler<Request>
        {
            private readonly AppDbContext _dbContext;
            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task Handle(Request request, CancellationToken cancellationToken)
            {

                var item = await _dbContext.Auctions
                    .AsNoTracking()
                    .FirstAsync(x => x.Id == request.AuctionId);

                var nowDate = DateTime.Now;

                var status = new AuctionStatus()
                {
                    ActionDate = nowDate,
                    AuctionId = request.AuctionId,
                    Type = AuctionStatusType.Finished,

                };

                var offer = new AuctionOffers()
                {
                    Date = nowDate,
                    AuctionId = request.AuctionId,
                    UserId = request.UserId,
                    PriceInstant = item.PriceInstant,

                };
                _dbContext.AuctionOffers.Add(offer);
                _dbContext.AuctionStatuses.Add(status);
                await _dbContext.SaveChangesAsync(cancellationToken);


            }
        }


    }
}
