using DB.Domain;
using DB.Domain.Entities;
using DB.Models;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Queries
{

    public class GetListOfAuctions
    {
        public class Request : IRequest<List<Response>>
        {

        }

        public class Response : IItem
        {
            public byte[]? Minature { get; set; }
            public string Title { get; set; }
            public int? AuctionId { get; set; } = null;

            public string? Extension { get; set; }

            public string OwnerName { get; set; }
            public int? OwnerId { get; set; } = null;
            public DateTime DateFinish { get; set; }

            public DateTime DateStarted { get; set; }
            public DB.Domain.Entities.AuctionType AuctionType { get; set; }
            public AuctionType Type { get; set; }
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
                var aukcje = await _dbContext.Auctions
                      .AsNoTracking()
                      .Include(x => x.Offers)
                      .Include(x => x.Attachments)
                      .Include(x => x.Status)
                      .Include(x => x.User)
                      .Where(x => x.Status.Any(x => x.Type == Domain.Entities.AuctionStatusType.Finished) == false)
                      .ToListAsync(cancellationToken);

                var aukcjeDoUsuniecia = aukcje.Where(x => x.AuctionFinish < DateTime.Now);

                foreach (var aukcja in aukcjeDoUsuniecia)
                {
                    if (aukcja.Offers.Any())
                    {
                        var najwyzszaOferta = aukcja.Offers.OrderByDescending(x => x.PriceAuction).FirstOrDefault();
                        aukcja.BuyerId = najwyzszaOferta.UserId;
                        aukcja.Status.Add(new Domain.Entities.AuctionStatus()
                        {
                            AuctionId = aukcja.Id,
                            Type = Domain.Entities.AuctionStatusType.Finished,
                            ActionDate = DateTime.Now,
                        });
                    }
                    _dbContext.Auctions.Update(aukcja);

                    await _dbContext.SaveChangesAsync(cancellationToken);
                }


                if (aukcje.Any() == false)
                    return new List<Response>();

                return aukcje.Where(x => aukcjeDoUsuniecia.Select(y => y.Id).Contains(x.Id) == false).Select(x => new Response()
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
                    AuctionType = x.Type


                }).ToList();

            }
        }


    }
}
