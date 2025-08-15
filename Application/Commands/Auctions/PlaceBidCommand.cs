using MediatR;
using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Application.Commands.Auctions
{
    public record PlaceBidCommand(
        int AuctionId,
        string SellerId,
        decimal BidAmount
    ) : IRequest<PlaceBidResult>;

    public record PlaceBidResult(
        bool Success,
        string? ErrorMessage = null,
        int? BidId = null
    );
}