using DB.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Queries
{
    public class GetAuctionsIWon
    {
        public class Request : IRequest<List<GetListOfAuctions.Response>>
        {
            public int UserId { get; set; }
        }


        private class Handler : IRequestHandler<Request, List<GetListOfAuctions.Response>>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;

            }
            public async Task<List<GetListOfAuctions.Response>> Handle(Request request, CancellationToken cancellationToken)
            {

                try
                {
                    var w = await _dbContext.Auctions
                 .AsNoTracking()
                 .Where(x => (x.Status.Any(y => y.Type == Domain.Entities.AuctionStatusType.Finished)
                       || x.Payments.Any(y => y.StripeStatus == Domain.Entities.StripeStatus.Success))
                       && x.BuyerId == request.UserId)
                 .Select(x => new GetListOfAuctions.Response()
                 {
                     Minature = x.Attachments.Any()
                                 ? x.Attachments.FirstOrDefault().ImageData
                                 : null,

                     Extension = x.Attachments.Any()
                                 ? x.Attachments.FirstOrDefault().Extension
                                 : null,

                     Title = x.Title,
                     AuctionId = x.Id,
                     OwnerName = x.User.Username,
                     OwnerId = x.User.Id,
                     DateFinish = x.AuctionFinish,
                     DateStarted = x.AuctionStart,
                     Type = x.Type,
                     Status = x.Payments.FirstOrDefault(x => x.StripeStatus == Domain.Entities.StripeStatus.Success) == null
                                        ? Domain.Entities.AuctionStatusType.Finished
                                        : Domain.Entities.AuctionStatusType.PaymentConfirmed

                 }).ToListAsync();
                    return w;
                }
                catch (Exception ex)
                {


                }

                return new List<GetListOfAuctions.Response>();



            }
        }


    }
}
