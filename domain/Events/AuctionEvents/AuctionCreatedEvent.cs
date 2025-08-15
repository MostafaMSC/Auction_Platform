using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Events.AuctionEvents
{
    public record AuctionCreatedEvent(int AuctionId, int ProjectId, DateTime StartAt, DateTime EndAt) : DomainEvent;
}
