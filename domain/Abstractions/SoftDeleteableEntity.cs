namespace AuctionSystem.Domain.Abstractions
{
    // كلاس أساسي للكيانات التي تدعم عدم الحذف الفعلي للبيانات (Soft Delete)
    public abstract class SoftDeletableEntity : Entity
    {
        // حالة الحذف: true إذا تم الحذف، false إذا لا
        public bool IsDeleted { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        protected abstract bool IsDeletable();

        public void SoftDelete()
        {
            if (!IsDeletable())
                throw new InvalidOperationException("Cannot delete entity due to domain rules.");

            // وضع العلامة على أنه تم الحذف وتسجيل وقت الحذف
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
