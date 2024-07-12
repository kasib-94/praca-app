using DB.Domain;

using MediatR;

namespace DB.Modules.Payment.Queries
{

    public class MakePayment
    {
        public class Request : IRequest
        {
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
                try
                {
                    Stripe.StripeConfiguration.ApiKey = "sk_test_51PbgpxCs4NH2MtqA4UPZY8vWMu1s32lAKZQWCbjxfZtgWbFLF5v7JPqo5UhmQ2IGmUiH7qHZXhN5ajvStlLIlr8E000nKghW8M";
                    var options = new Stripe.Checkout.SessionCreateOptions
                    {
                        SuccessUrl = "https://example.com/success",
                        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {

                        },
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                            Quantity = 2,
                        },
                    },
                        Mode = "payment",
                    };
                    var service = new Stripe.Checkout.SessionService();
                    service.Create(options);
                }
                catch (Exception)
                {

                    throw;
                }


                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }


    }
}
