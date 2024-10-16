using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface INotificationService
{
    Task<Result<IEnumerable<NotificationDto>>> GetNotificationsWithDetailsByUserId(int userId);
    Task<Result<IEnumerable<NotificationDto>>> GetUnreadNotificationsForUserAsync(int userId);
}
