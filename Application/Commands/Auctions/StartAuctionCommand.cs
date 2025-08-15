using MediatR;

namespace AuctionSystem.Application.Commands.Auctions
{
    public record StartAuctionCommand(int AuctionId) : IRequest<StartAuctionResult>;

    public record StartAuctionResult(bool Success, string? ErrorMessage = null);
}
