using MediatR;

namespace AuctionSystem.Application.Commands.Notification
{
    // Create Notification Command
    public class CreateNotificationCommand : IRequest<int> // int وليس Guid
    {
        public string UserId { get; set; }
        public string Message { get; set; }

        public CreateNotificationCommand(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }

    // Mark as Read Command
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public int NotificationId { get; set; }

        public MarkNotificationAsReadCommand(int notificationId)
        {
            NotificationId = notificationId;
        }
    }

    // Mark All as Read Command
    public class MarkAllNotificationsAsReadCommand : IRequest<int>
    {
        public string UserId { get; set; }

        public MarkAllNotificationsAsReadCommand(string userId)
        {
            UserId = userId;
        }
    }
}
