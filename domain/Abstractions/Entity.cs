using AuctionSystem.Domain.Events;

namespace AuctionSystem.Domain.Abstractions
{
    // الكلاس الأساسي لجميع الكيانات في Domain
    public abstract class Entity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

        // قائمة الأحداث الخاصة بالكيان (Domain Events)
        private readonly List<IDomainEvent> _domainEvents = new();

        // قراءة فقط للأحداث (لضمان عدم تعديلها من خارج الكلاس)
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // رفع حدث جديد للكيان (يتم تخزينه في القائمة الداخلية)
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // مسح جميع الأحداث بعد معالجتها (عادة بعد إرسالها عبر Mediator أو Event Dispatcher)
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
