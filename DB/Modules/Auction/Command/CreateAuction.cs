﻿using DB.Domain;
using DB.Domain.Entities;

using FluentValidation;

using MediatR;

namespace DB.Modules.Auction.Command
{
    public class CreateAuction
    {
        public class Request : IRequest<int>
        {
            public int UserId { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public decimal PriceAuction { get; set; } = decimal.Zero;
            public decimal PriceInstant { get; set; } = decimal.Zero;

            public AuctionType Type { get; set; }
            public List<DB.Models.Photo> Photos { get; set; } = new();


        }
        private class Handler : IRequestHandler<Request, int>
        {
            private readonly AppDbContext _dbContext;
            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;

            }
            public async Task<int> Handle(Request request, CancellationToken cancellationToken)
            {
                var auctionGuid = Guid.NewGuid().ToString();

                DateTime auctionStart = DateTime.Now;
                DateTime auctionFinish = DateTime.Now;
                if (request.Type == AuctionType.Days_14 || request.Type == AuctionType.Days_14_With_Instant)
                    auctionFinish = auctionFinish.AddDays(14);

                if (request.Type == AuctionType.Days_7 || request.Type == AuctionType.Days_7_With_Instant || request.Type == AuctionType.Instant)
                    auctionFinish = auctionFinish.AddDays(7);

                var auction = new DB.Domain.Entities.Auction()
                {
                    Type = request.Type,
                    AuctionGuid = auctionGuid,
                    Description = request.Description,
                    PriceAuctionStart = request.PriceAuction,
                    PriceInstant = request.PriceInstant,
                    Title = request.Title,
                    AuctionFinish = auctionFinish,
                    AuctionStart = auctionStart,
                    UserId = request.UserId,
                    Attachments = request.Photos.Select(x => new AuctionAttachment()
                    {
                        ImageData = x.ImageData,
                        Order = x.Order,
                        Miniature = x.Minature,
                        Extension = x.Extension,

                    }).ToList(),
                    Status = new List<Domain.Entities.AuctionStatus>()
                    {
                         new AuctionStatus(){
                             ActionDate = auctionStart,
                             Type = AuctionStatusType.Start
                         }
                    }

                };


                _dbContext.Auctions.Add(auction);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return auction.Id;


            }
        }

        internal class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Photos).NotEmpty().WithMessage("You have to add at least one photo");
                RuleFor(x => x.Title).NotEmpty().WithMessage("You have to add title");
                RuleFor(x => x.Title).MinimumLength(5).WithMessage("Title must have at least 5 letters");
                RuleFor(x => x.Type).Must(x => (int)x > 0).WithMessage("You have to choose type of auction");
                RuleFor(x => x.Description).MaximumLength(20).WithMessage("Description at least 20 letters");

                RuleFor(x => x.PriceInstant).Must(x => x > 0)
                    .When(x => DB.SD.AuctionSD.IsInstantBuy(x.Type))
                    .WithMessage("Musisz ustawić cenę kup teraz");

                RuleFor(x => x.PriceInstant).Must(x => x > 0)
                   .When(x => DB.SD.AuctionSD.IsAuction(x.Type))
                   .WithMessage("Musisz ustawić cenę wywoławczą");

                RuleFor(x => x).Must(x => x.PriceAuction < x.PriceInstant)
                    .When(x => x.Type == AuctionType.Days_7_With_Instant || x.Type == AuctionType.Days_14_With_Instant)
                    .WithMessage("Cena kup teraz musi być większa od ceny wywoławczej");


            }
        }
    }
}
