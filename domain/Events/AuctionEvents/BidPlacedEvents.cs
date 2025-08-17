using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Events.AuctionEvents
{
public record BidPlacedEvent(int AuctionId, string BidderId, string AuctionOwnerId, Money Amount) : DomainEvent;
    public record AuctionPriceDecreasedEvent(int AuctionId, Money OldPrice, Money NewPrice) : DomainEvent;
    public record AuctionCancelledEvent(int AuctionId) : DomainEvent;
    
}



