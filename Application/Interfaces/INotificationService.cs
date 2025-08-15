// File: Application/Interfaces/INotificationService.cs
namespace AuctionSystem.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message);
    }
}
