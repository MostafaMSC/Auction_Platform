using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Events.AuctionEvents
{
    public record AuctionClosedEvent(int AuctionId, int? WinningBidId, string? WinnerId, Money? WinningAmount) : DomainEvent;
    public record WinnerSelectedEvent(int AuctionId, int WinningBidId, string WinnerId, Money WinningAmount) : DomainEvent;
}
