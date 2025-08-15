namespace AuctionSystem.Domain.Events.ProjectEvents
{

    public record ProjectSubmittedEvent(
        int ProjectId,
        string OwnerId
    ) : DomainEvent;
public record ProjectAuctionCreatedEvent(int ProjectId, int AuctionId) : DomainEvent;
    public record ProjectAuctionCompletedEvent(int ProjectId) : DomainEvent;
}