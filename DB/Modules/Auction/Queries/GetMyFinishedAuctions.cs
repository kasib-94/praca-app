using DB.Domain;
using DB.Domain.Entities;
using DB.Models;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Queries
{

    public class GetMyFinishedAuctions
    {
        public class Request : IRequest<List<Response>>
        {
            public int UserId { get; set; }
        }

        public class Response : IItem
        {
            public byte[]? Minature { get; set; }
            public string Title { get; set; }
            public int? AuctionId { get; set; }

            public string? Extension { get; set; }

            public string? BuyerName { get; set; }
            public int? BuyerId { get; set; }
            public DateTime DateFinish { get; set; }

            public int? OwnerId { get; set; }
            public string OwnerName { get; set; }

            public AuctionType Type { get; set; }
            public DB.Domain.Entities.AuctionStatusType Status { get; set; }

        }
        private class Handler : IRequestHandler<Request, List<Response>>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;

            }
            public async Task<List<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _dbContext.Auctions
                      .AsNoTracking()
                      .Where(x => x.Status.Any(x => x.Type == Domain.Entities.AuctionStatusType.Finished)
                            && x.UserId == request.UserId)
                      .Select(x => new Response()
                      {
                          Minature = x.Attachments.Any()
                                      ? x.Attachments.FirstOrDefault().ImageData
                                      : null,

                          Extension = x.Attachments.Any()
                                      ? x.Attachments.FirstOrDefault().Extension
                                      : null,

                          Title = x.Title,
                          AuctionId = x.Id,
                          DateFinish = x.Status.First(y => y.Type == Domain.Entities.AuctionStatusType.Finished).ActionDate,
                          Type = x.Type,
                          BuyerName = x.Buyer == null
                                    ? null
                                    : x.Buyer.Username,
                          BuyerId = x.Buyer == null
                                    ? null
                                    : x.Buyer.Id,
                          OwnerId = x.UserId,
                          Status = x.Payments.Any(x => x.StripeStatus == StripeStatus.Success)
                                     ? AuctionStatusType.PaymentConfirmed
                                     : AuctionStatusType.Finished




                      })
                      .ToListAsync(cancellationToken);

            }
        }


    }
}
