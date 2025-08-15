using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Application.Extensions
{
    public static class AuctionMappingExtensions
    {
        public static AuctionDto ToDto(this Auction auction)
        {
            return new AuctionDto(
                auction.Id,
                auction.ProjectId,
                auction.CurrentPrice.Amount,
                auction.StartingPrice.Amount,
                auction.MinPrice.Amount,
                auction.TargetPrice.Amount,
                auction.StartAt,
                auction.EndAt,
                auction.Status.ToString(),
                auction.IsActive,
                auction.Bids.Select(b => b.ToDto())
                           .OrderByDescending(b => b.CreatedAt)
                           .ToList()
            );
        }

        public static BidDto ToDto(this Bid bid)
        {
            return new BidDto(
                bid.Id,
                bid.SellerId,
                bid.Amount,
                bid.CreatedAt
            );
        }
    }
}