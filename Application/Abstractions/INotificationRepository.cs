using Application.DTOs;

namespace Application.Abstractions;

public interface INotificationRepository : ICRUDRepository<NotificationDto, int>
{
    Task<IEnumerable<NotificationDto>> GetNotificationsWithDetailsByUserId(int userId);
}
