using MediatR;

namespace AuctionSystem.Domain.Events
{
    // Inherit from INotification for MediatR integration
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
        Guid EventId { get; }
    }
    
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid EventId { get; } = Guid.NewGuid();
    }
}