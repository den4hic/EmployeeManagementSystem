using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface INotificationService
{
    Task<Result<IEnumerable<NotificationDto>>> GetNotificationsWithDetailsByUserId(int userId);
}
