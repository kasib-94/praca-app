using DB.Domain;
using DB.Domain.Entities;
using DB.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Stripe.Checkout;

namespace DB.Modules.Payment.Queries
{

    public class MakePayment
    {
        public class Request : IRequest<StripeSessionDto>
        {
            public int AuctionId { get; set; }

        }

        private class Handler : IRequestHandler<Request, StripeSessionDto>
        {
            private readonly AppDbContext _dbContext;
            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }



            public async Task<StripeSessionDto> Handle(Request request, CancellationToken cancellationToken)
            {

                var stripeSessionDto = new StripeSessionDto();
                var auction = _dbContext.Auctions
                    .AsNoTracking()
                    .Include(x => x.Offers)
                    .First(x => x.Id == request.AuctionId);
                try
                {
                    Stripe.StripeConfiguration.ApiKey = "sk_test_51PbgpxCs4NH2MtqA4UPZY8vWMu1s32lAKZQWCbjxfZtgWbFLF5v7JPqo5UhmQ2IGmUiH7qHZXhN5ajvStlLIlr8E000nKghW8M";
                    var options = new Stripe.Checkout.SessionCreateOptions
                    {
                        CancelUrl = "https://localhost:7001/successpayment/" + auction.Id,
                        SuccessUrl = "https://localhost:7001/successpayment/" + auction.Id,
                        Mode = "payment",
                        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>()


                    };
                    var item = new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = DB.SD.AuctionSD.IsInstantBuy(auction.Type)
                                        ? (long)auction.PriceInstant
                                        : (long)auction.Offers.Max(x => x.PriceAuction),
                            Currency = "pln",

                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = auction.Title,

                            }

                        },
                        Quantity = 1,


                    };
                    options.LineItems.Add(item);
                    var service = new SessionService();
                    Session session = service.Create(options);

                    var sessionEntity = new StripeSession()
                    {
                        AuctionId = request.AuctionId,
                        SessionId = session.Id,
                        SessionUrl = session.Url,
                        StripeStatus = StripeStatus.Pending
                    };
                    _dbContext.StripeSessions.Add(sessionEntity);
                    await _dbContext.SaveChangesAsync();


                    stripeSessionDto.StripeSessionUrl = session.Url;
                    stripeSessionDto.StripeSessionId = session.Id;


                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                return stripeSessionDto;

            }
        }


    }
}
