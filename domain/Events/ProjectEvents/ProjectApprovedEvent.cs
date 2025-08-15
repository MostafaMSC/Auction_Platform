namespace AuctionSystem.Domain.Events.ProjectEvents
{
    public record ProjectApprovedEvent(
        int ProjectId,
        int OwnerId,
        int ApprovedById
    ) : DomainEvent;
}