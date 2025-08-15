// File: Infrastructure/Services/NotificationService.cs
using AuctionSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuctionSystem.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendNotificationAsync(string userId, string message)
        {
            // For now, just log to console
            Console.WriteLine($"Notify user {userId}: {message}");
            return Task.CompletedTask;
        }
    }
}
