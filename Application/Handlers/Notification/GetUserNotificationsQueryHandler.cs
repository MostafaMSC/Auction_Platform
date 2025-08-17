using MediatR;
using AuctionSystem.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuctionSystem.Application.Queries.Notification;

namespace AuctionSystem.Application.Handlers
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = request.UnreadOnly 
                ? await _notificationRepository.GetUnreadByUserIdAsync(request.UserId)
                : await _notificationRepository.GetByUserIdAsync(request.UserId);

            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).OrderByDescending(n => n.CreatedAt);
        }
    }

    
}