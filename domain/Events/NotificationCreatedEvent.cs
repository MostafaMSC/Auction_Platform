using AuctionSystem.Domain.Events;

public record NotificationCreatedEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Title { get; }
    public string Message { get; }

    public NotificationCreatedEvent(Guid userId, string title, string message)
    {
        UserId = userId;
        Title = title;
        Message = message;
    }
}