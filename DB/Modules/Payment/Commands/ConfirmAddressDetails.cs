using DB.Domain;
using DB.Domain.Entities;

using MediatR;

namespace DB.Modules.Payment.Commands
{
    public class ConfirmAddressDetails
    {
        public class Request : IRequest
        {
            public DB.Modules.Payment.Queries.GetPostalData.Response Model { get; set; }

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
                if (string.IsNullOrEmpty(request.Model.PostCode) || string.IsNullOrEmpty(request.Model.Adress) || string.IsNullOrEmpty(request.Model.PhoneNumber))
                {
                    throw new Exception("Fill data first!");
                }
                var entity = new PostalData()
                {
                    AuctionId = request.Model.AuctionId,
                    PhoneNumber = request.Model.PhoneNumber,
                    Adress = request.Model.Adress,
                    Confirmed = true,
                    PostCode = request.Model.PostCode,
                    UserId = request.Model.UserId,
                };

                _dbContext.PostalData.Update(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }


    }
}
