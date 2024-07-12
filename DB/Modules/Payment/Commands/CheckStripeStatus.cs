using DB.Domain;
using DB.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Stripe.Checkout;

namespace DB.Modules.Payment.Commands
{
    public class CheckStripeStatus
    {
        public class Request : IRequest<StripeStatus>
        {
            public int AuctionId { get; set; }
        }

        private class Handler : IRequestHandler<Request, StripeStatus>
        {
            private readonly AppDbContext _dbContext;
            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<StripeStatus> Handle(Request request, CancellationToken cancellationToken)
            {
                var pending = await _dbContext.StripeSessions
                      .AsNoTracking()
                      .Where(x => x.AuctionId == request.AuctionId && x.StripeStatus == Domain.Entities.StripeStatus.Pending)
                      .ToListAsync();

                SessionService session = new();
                StripeStatus wynik = StripeStatus.Failed;
                foreach (var item in pending)
                {
                    var result = await session.GetAsync(item.SessionId);
                    if (result.PaymentStatus == "paid")
                    {
                        item.StripeStatus = Domain.Entities.StripeStatus.Success;
                        wynik = StripeStatus.Success;
                    }
                    else
                    {
                        item.StripeStatus = StripeStatus.Failed;

                    }
                    _dbContext.StripeSessions.Update(item);
                }
                await _dbContext.SaveChangesAsync(cancellationToken);
                return wynik;

            }
        }


    }
}
