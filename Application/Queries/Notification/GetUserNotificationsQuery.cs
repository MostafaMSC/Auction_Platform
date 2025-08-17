using MediatR;
using System.Collections.Generic;

namespace AuctionSystem.Application.Queries.Notification
{
    public class GetUserNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
    {
        public string UserId { get; set; }
        public bool UnreadOnly { get; set; }

        public GetUserNotificationsQuery(string userId, bool unreadOnly = false)
        {
            UserId = userId;
            UnreadOnly = unreadOnly;
        }
    }
}
