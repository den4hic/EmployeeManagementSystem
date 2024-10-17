using Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("get-notifications/{userId}")]
    public async Task<IActionResult> GetNotifications(int userId)
    {
        var notifications = await _notificationService.GetNotificationsWithDetailsByUserId(userId);

        if (notifications.IsSuccess)
        {
            return Ok(notifications.Value);
        }

        return NotFound(notifications.Error);
    }

    [HttpGet("get-unread-notifications/{userId}")]
    public async Task<IActionResult> GetUnreadNotifications(int userId)
    {
        var notifications = await _notificationService.GetUnreadNotificationsForUserAsync(userId);

        if (notifications.IsSuccess)
        {
            return Ok(notifications.Value);
        }

        return NotFound(notifications.Error);
    }

    [HttpPut("mark-notification-as-read/{notificationId}/{userId}")]
    public async Task<IActionResult> MarkNotificationAsRead(int notificationId, int userId)
    {
        var result = await _notificationService.MarkNotificationAsReadAsync(notificationId, userId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }
}
