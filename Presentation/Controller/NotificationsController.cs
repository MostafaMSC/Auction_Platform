using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem.Application.Queries.Notification;
using AuctionSystem.Application.Commands.Notification;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// إنشاء إشعار جديد للمستخدم.
    /// </summary>
    /// <param name="command">يحتوي على UserId و Message للإشعار</param>
    /// <returns>معرّف الإشعار الذي تم إنشاءه</returns>
    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Success = true, NotificationId = id });
    }

    /// <summary>
    /// الحصول على جميع الإشعارات الخاصة بالمستخدم الحالي.
    /// </summary>
    /// <param name="unreadOnly">إذا true يتم جلب الإشعارات غير المقروءة فقط</param>
    /// <returns>قائمة من الإشعارات</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserNotifications([FromQuery] bool unreadOnly = false)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetUserNotificationsQuery(userId, unreadOnly);
        var notifications = await _mediator.Send(query);
        return Ok(notifications);
    }

    /// <summary>
    /// تعليم إشعار معين كمقروء.
    /// </summary>
    /// <param name="notificationId">معرّف الإشعار الذي سيتم تعليمه كمقروء</param>
    /// <returns>Success = true إذا تمت العملية بنجاح</returns>
    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var command = new MarkNotificationAsReadCommand(notificationId);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true }) : NotFound(new { Success = false });
    }
}
