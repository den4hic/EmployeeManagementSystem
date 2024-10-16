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
            .AsNoTracking()
            .Include(n => n.Group)
            .Include(n => n.Receiver)
            .Include(n => n.Sender)
            .ThenInclude(s => s.UserPhoto)
            .Include(n => n.UserNotifications)
            .ThenInclude(un => un.User)
            .Where(n => n.UserNotifications.Any(un => un.UserId == userId));

        return await notifications.Select(n => _mapper.Map<NotificationDto>(n)).ToListAsync();
    }

    public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsForUserAsync(int userId)
    {
        var unreadNotifications = await _context.UserNotifications
            .Where(un => un.UserId == userId && !un.IsRead)
            .Include(un => un.Notification)
            .ThenInclude(n => n.Sender)
            .ThenInclude(s => s.UserPhoto)
            .Select(un => un.Notification)
            .ToListAsync();

        return _mapper.Map<IEnumerable<NotificationDto>>(unreadNotifications);
    }
}
