using DB.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Payment.Queries
{

    public class GetPostalData
    {
        public class Request : IRequest<Response>
        {
            public int AuctionId { get; set; }
            public int UserId { get; set; }

        }

        public class Response
        {

            public int Id { get; set; }
            public string BuyerUsername { get; set; } = "";
            public int UserId { get; set; }
            public int AuctionId { get; set; }

            public string Adress { get; set; } = "";
            public string PostCode { get; set; } = "";

            public bool Confirmed { get; set; } = false;
            public string PhoneNumber { get; set; } = "";
            public int BuyerId { get; set; }
            public bool BrakDostepu { get; set; } = false;

            public bool FormEnabled => Confirmed == false;
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
                var auction = _dbContext.Auctions
                    .FirstOrDefault(x => x.Id == request.AuctionId);

                if (auction.BuyerId.HasValue == false || auction.BuyerId.Value != request.UserId)
                {
                    return new Response()
                    {
                        BrakDostepu = true
                    };
                }

                var item = _dbContext.PostalData
                     .Include(x => x.User)
                    .FirstOrDefault(x => x.UserId == request.UserId && x.AuctionId == request.AuctionId);

                if (item == null)
                {
                    return new Response();
                }
                else
                {
                    return new Response()
                    {
                        Adress = item.Adress,
                        AuctionId = item.AuctionId,
                        BuyerId = auction.BuyerId.Value,
                        PhoneNumber = item.PhoneNumber,
                        PostCode = item.PostCode,
                        Confirmed = item.Confirmed,
                        UserId = item.UserId,
                        BuyerUsername = item.User.Username
                    };
                }
            }
        }


    }
}
