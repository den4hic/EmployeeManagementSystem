using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

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
}
