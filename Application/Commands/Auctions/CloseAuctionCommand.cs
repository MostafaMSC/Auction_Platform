using MediatR;

namespace AuctionSystem.Application.Commands.Auctions
{
    public record CloseAuctionCommand(
        int AuctionId,
        string UserId
    ) : IRequest<CloseAuctionResult>; // <- implement IRequest<T>
    
    public record CloseAuctionResult(bool Success, string? ErrorMessage = null);
}
