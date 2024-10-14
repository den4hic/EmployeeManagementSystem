using Application.DTOs;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

namespace WebAPI.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private static readonly Dictionary<string, string> OnlineUsers = new Dictionary<string, string>();
    private readonly EmployeeManagementSystemDbContext _context;

    public NotificationHub(EmployeeManagementSystemDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        string userId = GetUserId(Context.User);
        string connectionId = Context.ConnectionId;
        var user = await _context.Users
            .Include(u => u.UserNotificationGroups)
            .ThenInclude(ung => ung.Group)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (user != null)
        {
            foreach (var userGroup in user.UserNotificationGroups)
            {
                await Groups.AddToGroupAsync(connectionId, userGroup.Group.Name);
            }
        }

        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(connectionId))
            {
                OnlineUsers.Add(connectionId, userId);
            }
        }

        await UpdateUserList();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string userId = GetUserId(Context.User);
        string connectionId = Context.ConnectionId;
        var user = await _context.Users
            .Include(u => u.UserNotificationGroups)
            .ThenInclude(ung => ung.Group)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (user != null)
        {
            foreach (var userGroup in user.UserNotificationGroups)
            {
                await Groups.RemoveFromGroupAsync(connectionId, userGroup.Group.Name);
            }
        }

        lock (OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(connectionId))
            {
                OnlineUsers.Remove(connectionId);
            }
        }

        await UpdateUserList();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task CreateProjectGroup(string id)
    {
        int projectId = int.Parse(id);
        var project = await _context.Projects
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employee)
            .ThenInclude(e => e.User)
            .Include(p => p.ProjectManagers)
            .ThenInclude(pm => pm.Manager)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            return;
        var groupName = $"Project_{projectId}";
        var group = new NotificationGroup { Name = groupName };

        var usersToAdd = project.ProjectEmployees.Select(pe => pe.Employee.User)
            .Concat(project.ProjectManagers.Select(pm => pm.Manager.User))
            .Distinct();

        foreach (var user in usersToAdd)
        {
            group.UserNotificationGroups.Add(new UserNotificationGroups { User = user });
            if (OnlineUsers.ContainsValue(user.Id.ToString()))
            {
                var connectionKvp = OnlineUsers.FirstOrDefault(kvp => kvp.Value == user.Id.ToString());
                if (connectionKvp.Key != null)
                {
                    await Groups.AddToGroupAsync(connectionKvp.Key, groupName);
                }
            }
        }

        _context.NotificationGroups.Add(group);
        await _context.SaveChangesAsync();
    }

    public async Task GetGroupNotifications(int groupId)
    {
        var userId = GetUserId(Context.User);
        var user = await _context.Users
            .Include(u => u.UserNotificationGroups)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (user == null || !user.UserNotificationGroups.Any(ung => ung.GroupId == groupId))
            return;

        var notifications = await _context.Notifications
            .Where(n => n.GroupId == groupId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                GroupId = n.GroupId,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                Type = n.Type
            })
            .ToListAsync();

        await Clients.Caller.SendAsync("ReceiveGroupNotifications", notifications);
    }

    public async Task SendProjectNotification(ProjectDto project, NotificationType type)
    {
        var groupName = $"Project_{project.Id}";
        var group = await _context.NotificationGroups
            .FirstOrDefaultAsync(g => g.Name == groupName);

        if (group == null)
            return;

        var notification = new Notification
        {
            GroupId = group.Id,
            CreatedAt = DateTime.UtcNow,
            Type = type
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var notificationDto = new NotificationDto
        {
            Id = notification.Id,
            GroupId = notification.GroupId,
            CreatedAt = notification.CreatedAt,
            Type = notification.Type
        };

        await Clients.Group(groupName).SendAsync("ReceiveNotification", type, project.Name);
    }


    public async Task SendNotification(string userId, NotificationType notificationType, TaskDto task)
    {
        var connectionIds = OnlineUsers.Where(kvp => kvp.Value == userId).Select(kvp => kvp.Key).ToList();
        await Clients.Clients(connectionIds).SendAsync("ReceiveNotification", notificationType, task.Title);
    }

    private async Task UpdateUserList()
    {
        List<string> users;
        lock (OnlineUsers)
        {
            users = OnlineUsers.Values.Distinct().ToList();
        }
        await Clients.All.SendAsync("UpdateUserList", users);
    }

    private string GetUserId(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0";
    }
}