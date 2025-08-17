using MediatR;

using AuctionSystem.Application.Commands.Notification;
namespace AuctionSystem.Application.Handlers.m
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification(request.UserId, request.Message);
            await _notificationRepository.AddAsync(notification);
            return notification.Id; // متوافق مع int
        }
    }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
            if (notification == null) return false;

            notification.MarkAsRead();
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }
    }

    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetUnreadByUserIdAsync(request.UserId);
            int count = 0;

            foreach (var notification in notifications)
            {
                notification.MarkAsRead();
                await _notificationRepository.UpdateAsync(notification);
                count++;
            }

            return count;
        }
    }
}
