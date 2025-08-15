namespace AuctionSystem.Domain.Events.UserEvents
{
    public record UserVerifiedEvent(
        string UserId,
        string FullName
    ) : DomainEvent;
}