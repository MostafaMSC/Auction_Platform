using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task CreateAsync(Notification notification);
        Task<bool> UpdateAsync(Notification notification);
        Task MarkAsReadAsync(int notificationId);
        Task<bool> DeleteAsync(int id);
    }
}
