namespace AuctionSystem.Domain.Events.UserEvents
{
    public record UserRejectedEvent(
        string UserId,
        string Reason
    ) : DomainEvent;
}