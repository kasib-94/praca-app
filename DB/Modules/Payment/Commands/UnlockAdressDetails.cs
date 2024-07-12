using DB.Domain;

using MediatR;

namespace DB.Modules.Payment.Commands
{
    public class UnlockAdressDetails
    {
        public class Request : IRequest
        {
            public int PostDataId { get; set; }

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
                var item = _dbContext.PostalData
     .FirstOrDefault(x => x.Id == request.PostDataId);

                if (item != null)
                {
                    _dbContext.PostalData.Remove(item);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }


    }
}
