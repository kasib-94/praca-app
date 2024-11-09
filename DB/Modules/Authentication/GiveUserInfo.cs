using DB.Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Authentication
{
    public class GiveUserInfo
    {
        public class Request : IRequest<Response>
        {
            public int UserId { get; set; }

        }

        public class Response
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public int AuctionsActive { get; set; }
            public int AuctionsSold { get; set; }
            public int AuctionsNotSold { get; set; }

            public decimal Score { get; set; }
        }

        private class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext _dbContext;
            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                return (await _dbContext.Users
                    .AsNoTracking()
                    .Where(x => x.Id == request.UserId)
                    .Select(x => new Response()
                    {
                        Id = x.Id,
                        UserName = x.Username,
                        Email = x.Email,
                        AuctionsActive = x.Auctions.Where(y => y.Status.Any(w => w.Type == Domain.Entities.AuctionStatusType.Finished) == false).Count(),
                        AuctionsNotSold = x.Auctions.Where(y => y.Status.Any(w => w.Type == Domain.Entities.AuctionStatusType.Finished) == true
                                                                              && y.BuyerId == null
                          ).Count(),
                        AuctionsSold = x.Auctions.Where(y => y.Status.Any(w => w.Type == Domain.Entities.AuctionStatusType.Finished) == true
                                                                              && y.BuyerId != null).Count(),



                    }).ToListAsync()).First();


            }
        }


    }
}
