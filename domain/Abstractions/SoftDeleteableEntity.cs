namespace AuctionSystem.Domain.Abstractions
{
    public abstract class SoftDeletableEntity : Entity
    {
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        protected abstract bool IsDeletable();

        public void SoftDelete()
        {
            if (!IsDeletable())
                throw new InvalidOperationException("Cannot delete entity due to domain rules.");

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
