using AuctionSystem.Domain.Entities;

public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(string userId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
    }