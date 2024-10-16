using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificationRepository : CRUDRepositoryBase<Notification, NotificationDto, EmployeeManagementSystemDbContext, int>, INotificationRepository
{
    public NotificationRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsWithDetailsByUserId(int userId)
    {
        var notifications = _context.Notifications
            .Include(n => n.Group)
            .Include(n => n.Receiver)
            .Include(n => n.Sender)
            .Include(n => n.UserNotifications)
            .ThenInclude(un => un.User)
            .Where(n => n.UserNotifications.Any(un => un.UserId == userId));

        foreach (var notification in notifications)
        {
            if (notification.UserNotifications != null)
            {
                notification.UserNotifications = null;
            }
            if (notification.Receiver != null)
            {
                notification.Receiver.NotificationReceivers = null;
                notification.Receiver.UserNotifications = null;
            }
            if (notification.Sender != null)
            {
                notification.Sender.NotificationSenders = null;
                notification.Sender.UserNotifications = null;
            }
            if (notification.Group != null)
            {
                notification.Group.Notifications = null;
            }
        }

        return await notifications.Select(n => _mapper.Map<NotificationDto>(n)).ToListAsync();
    }
}
