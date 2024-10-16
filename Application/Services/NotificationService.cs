using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<IEnumerable<NotificationDto>>> GetNotificationsWithDetailsByUserId(int userId)
    {
        var notifications = await _notificationRepository.GetNotificationsWithDetailsByUserId(userId);

        if (notifications == null)
        {
            return Result<IEnumerable<NotificationDto>>.Failure("Notifications not found.");
        }

        return Result<IEnumerable<NotificationDto>>.Success(notifications);
    }

    public async Task<Result<IEnumerable<NotificationDto>>> GetUnreadNotificationsForUserAsync(int userId)
    {
        var notifications = await _notificationRepository.GetUnreadNotificationsForUserAsync(userId);

        if (notifications == null)
        {
            return Result<IEnumerable<NotificationDto>>.Failure("Notifications not found.");
        }

        return Result<IEnumerable<NotificationDto>>.Success(notifications);
    }

    public async Task<Result<bool>> MarkNotificationAsReadAsync(int notificationId)
    {
        try
        {
            await _notificationRepository.MarkNotificationAsReadAsync(notificationId);

            return Result<bool>.Success(true);
        }
        catch (KeyNotFoundException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
}
