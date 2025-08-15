using MediatR;

namespace AuctionSystem.Application.Commands.Auctions
{
    public record CreateAuctionCommand(
        int ProjectId,
        decimal StartingPrice,
        decimal MinPrice,
        decimal TargetPrice,
        int MaxDurationHours,
        int PriceDropIntervalMinutes,
        decimal PriceDropAmount
    ) : IRequest<CreateAuctionResult>;

    public record CreateAuctionResult(
        bool Success,
        int? AuctionId = null,
        string? ErrorMessage = null
    );
}