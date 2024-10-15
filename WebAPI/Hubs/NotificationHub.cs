using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
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
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public NotificationHub(EmployeeManagementSystemDbContext context, IUserService userService, IProjectService projectService, IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _projectService = projectService;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        string userId = GetUserId(Context.User);
        string connectionId = Context.ConnectionId;
        var userResult = await _userService.GetUserByIdWithGroups(int.Parse(userId));

        if (userResult.IsSuccess)
        {
            var user = userResult.Value;

            if (user != null)
            {
                foreach (var userGroup in user.NotificationGroups)
                {
                    await Groups.AddToGroupAsync(connectionId, userGroup.Name);
                }
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
        var userResult = await _userService.GetUserByIdWithGroups(int.Parse(userId));

        if (userResult.IsSuccess)
        {
            var user = userResult.Value;

            if (user != null)
            {
                foreach (var userGroup in user.NotificationGroups)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, userGroup.Name);
                }
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
        var projectResult = await _projectService.GetProjectByIdWithDetailsAsync(projectId);

        if (!projectResult.IsSuccess)
            return;

        var project = projectResult.Value;

        var groupName = $"Project_{projectId}";
        var group = new NotificationGroup { Name = groupName };

        var usersToAdd = project.Employees.Select(pe => pe.User)
            .Concat(project.Managers.Select(pm => pm.User))
            .Distinct();

        foreach (var user in usersToAdd)
        {
            group.UserNotificationGroups.Add(new UserNotificationGroups { User = _mapper.Map<User>(user) });
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
            .Include(g => g.UserNotificationGroups)
            .ThenInclude(ung => ung.User)
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

        foreach (var userGroup in group.UserNotificationGroups)
        {
            var isUserOnline = OnlineUsers.ContainsValue(userGroup.User.Id.ToString());

            var userNotification = new UserNotification
            {
                UserId = userGroup.UserId,
                NotificationId = notification.Id,
                IsRead = isUserOnline,
                ReadAt = isUserOnline ? DateTime.UtcNow : null
            };

            _context.UserNotifications.Add(userNotification);
        }

        await _context.SaveChangesAsync();

        await Clients.Group(groupName).SendAsync("ReceiveNotification", type, project.Name);
    }


    public async Task SendNotification(string userId, NotificationType notificationType, TaskDto task)
    {
        var notification = new Notification
        {
            ReceiverId = int.Parse(userId),
            CreatedAt = DateTime.UtcNow,
            Type = notificationType
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var isUserOnline = OnlineUsers.ContainsValue(userId);

        var userNotification = new UserNotification
        {
            UserId = int.Parse(userId),
            NotificationId = notification.Id,
            IsRead = isUserOnline,
            ReadAt = isUserOnline ? DateTime.UtcNow : null
        };

        _context.UserNotifications.Add(userNotification);
        await _context.SaveChangesAsync();

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