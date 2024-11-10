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

            public decimal Kwota { get; set; }
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
                    .AsNoTracking()
                    .Include(x => x.Payments)
                    .Include(x => x.Buyer)
                    .Include(x => x.Offers)
                    .FirstOrDefault(x => x.Id == request.AuctionId);
                var result = new Response();


                var w = auction.Offers.Select(x => x.PriceInstant).Concat(auction.Offers.Select(x => x.PriceAuction)).Max();
                result.Kwota = w.Value;



                if (auction.Payments.Any(x => x.StripeStatus == Domain.Entities.StripeStatus.Success))
                {
                    var itemResult = _dbContext.PostalData
          .AsNoTracking()
           .Include(x => x.User)
          .FirstOrDefault(x => x.UserId == auction.BuyerId && x.AuctionId == request.AuctionId);

                    result.Adress = itemResult.Adress;
                    result.PhoneNumber = itemResult.PhoneNumber;
                    result.PostCode = itemResult.PostCode;
                }


                if (auction.BuyerId.HasValue == false || auction.BuyerId.Value != request.UserId)
                {
                    result.BrakDostepu = true;
                    return result;
                }

                var item = _dbContext.PostalData
          .AsNoTracking()
           .Include(x => x.User)
          .FirstOrDefault(x => x.UserId == request.UserId && x.AuctionId == request.AuctionId);

                if (item == null)
                {

                    result.AuctionId = request.AuctionId;
                    result.UserId = request.UserId;
                    result.BuyerUsername = auction.Buyer.Username;
                    result.BuyerId = auction.BuyerId.Value;

                    return result;
                }
                else
                {

                    result.Id = item.Id;
                    result.Adress = item.Adress;
                    result.AuctionId = item.AuctionId;
                    result.BuyerId = auction.BuyerId.Value;
                    result.PhoneNumber = item.PhoneNumber;
                    result.PostCode = item.PostCode;
                    result.Confirmed = item.Confirmed;
                    result.UserId = item.UserId;
                    result.BuyerUsername = item.User.Username;

                }
                return result;
            }
        }


    }
}
