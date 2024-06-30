using DB.Domain;
using DB.Domain.Entities;

using MediatR;

namespace DB.Modules.Auction.Command
{

    public class FinishMyAuction
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

                var auction = _dbContext.Auctions.First(x => x.Id == request.AuctionId);

                var status = new AuctionStatus()
                {
                    Type = AuctionStatusType.Finished,
                    ActionDate = DateTime.Now,
                    AuctionId = request.AuctionId,
                };

                _dbContext.AuctionStatuses.Add(status);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }


    }
}
