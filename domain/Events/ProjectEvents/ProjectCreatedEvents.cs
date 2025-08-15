namespace AuctionSystem.Domain.Events.ProjectEvents
{
    public record ProjectCreatedEvent(
        int ProjectId,
        int OwnerId,
        string ProjectTitle,
        int CategoryId
    ) : DomainEvent;

    
}