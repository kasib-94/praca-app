using DB.Domain;

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

        public class Response
        {
            public byte[]? Minature { get; set; }
            public string Title { get; set; }
            public int Id { get; set; }

            public string OwnerName { get; set; }
            public int OwnerId { get; set; }
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
                      .Where(x => x.Status.Any(x => x.Type == Domain.Entities.AuctionStatusType.Finished) == false)
                      .Select(x => new Response()
                      {
                          Minature = x.Attachments.Any()
                                      ? x.Attachments.FirstOrDefault(y => y.Miniature == true).ImageData
                                      : null,
                          Title = x.Title,
                          Id = x.Id,
                          OwnerName = x.User.Username,
                          OwnerId = x.User.Id

                      })
                      .ToListAsync(cancellationToken);

            }
        }


    }
}
