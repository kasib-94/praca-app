﻿using DB.Domain;
using DB.Models;
using DB.Modules.Auction.Command;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DB.Modules.Auction.Queries
{
    public class GetAuctionById
    {
        public class Request : IRequest<Response>
        {
            public int AuctionId { get; set; }
            public int UserId { get; set; }

        }



        public class Response
        {
            public int AuctionId { get; set; }
            public decimal? PriceAuction { get; set; }
            public decimal? PriceInstant { get; set; }
            public decimal? PriceAuctionStart { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DB.Domain.Entities.AuctionType AuctionType { get; set; }

            public int OwnerId { get; set; }
            public string OwnerUsername { get; set; }

            public DateTime AuctionStarted { get; set; }

            public DateTime AuctionFinish { get; set; }

            public List<Photo> Photos { get; set; } = new();

            public List<AuctionStatus> Statuses { get; set; } = new();


            public List<Offer> Offers { get; set; } = new();

            public IBaseRequest BuyNowOption { get; set; }

            public IBaseRequest AuctionOfferOption { get; set; }
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
                var item = (await _dbContext.Auctions
                     .AsNoTracking()
                     .Where(x => x.Id == request.AuctionId)
                     .Select(x => new Response()
                     {
                         AuctionId = x.Id,
                         AuctionFinish = x.AuctionFinish,
                         AuctionStarted = x.AuctionStart,
                         OwnerUsername = x.User.Username,
                         OwnerId = x.User.Id,


                         PriceAuctionStart = x.IsAuction ? x.PriceAuctionStart : null,
                         PriceAuction = x.IsAuction
                           ? (x.Offers.Any(z => z.PriceAuction.HasValue == true)
                                        ? x.Offers.Max(x => x.PriceAuction) : null)
                           : null,
                         PriceInstant = x.IsInstantBuy
                           ? (x.Offers.Any(z => z.PriceInstant.HasValue == true)
                                        ? x.Offers.Max(x => x.PriceInstant) : null)
                           : null,
                         Title = x.Title,
                         Description = x.Description,

                         AuctionType = x.Type,
                         Photos = x.Attachments.Select(y => new Photo()
                         {
                             Extension = y.Extension,
                             ImageData = y.ImageData,
                             Minature = y.Miniature,
                             Order = y.Order,

                         }).ToList(),

                         Statuses = x.Status.Select(y => new AuctionStatus()
                         {
                             ActionDate = y.ActionDate,
                             AuctionId = y.AuctionId,
                             Type = y.Type,
                         }).ToList(),
                         Offers = x.Offers.Select(y => new Offer()
                         {
                             AuctionId = y.AuctionId,
                             Date = y.Date,
                             Id = y.Id,
                             PriceAuction = y.PriceAuction,
                             PriceInstant = y.PriceInstant,
                             UserName = y.User.Username

                         }).ToList()




                     }).ToListAsync()).First();

                if (request.UserId != item.OwnerId && item.Statuses.FirstOrDefault(x => x.Type == Domain.Entities.AuctionStatusType.Finished) == null)
                {
                    if (DB.SD.AuctionSD.IsInstantBuy(item.AuctionType))
                    {
                        item.BuyNowOption = new BuyNow.Request()
                        {
                            AuctionId = item.AuctionId,
                            UserId = request.UserId
                        };
                    }
                    if (DB.SD.AuctionSD.IsAuction(item.AuctionType))
                    {
                        item.AuctionOfferOption = new MakeOffer.Request()
                        {
                            Price = 0,
                            AuctionId = item.AuctionId,
                            UserId = request.UserId
                        };
                    }
                }

                return item;
            }
        }


    }
}
