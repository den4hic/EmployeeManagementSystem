﻿using Application.Abstractions;
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
        var project = await _context.Projects
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employee)
            .ThenInclude(e => e.User)
            .Include(p => p.ProjectManagers)
            .ThenInclude(pm => pm.Manager)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

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

    public async Task SendProjectNotification(ProjectDto project, NotificationType type)
    {
        var groupName = $"Project_{project.Id}";
        var group = await _context.NotificationGroups
            .Include(g => g.UserNotificationGroups)
            .ThenInclude(ung => ung.User)
            .FirstOrDefaultAsync(g => g.Name == groupName);

        string userId = GetUserId(Context.User);

        if (group == null)
            return;

        var notification = new Notification
        {
            SenderId = int.Parse(userId),
            GroupId = group.Id,
            CreatedAt = DateTime.Now,
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
                ReadAt = isUserOnline ? DateTime.Now : null
            };

            _context.UserNotifications.Add(userNotification);
        }

        await _context.SaveChangesAsync();

        await Clients.Group(groupName).SendAsync("ReceiveNotification", type, project.Name);
    }

    public async Task SendNotification(string recieverUserId, NotificationType notificationType, string notificationTitle)
    {
        string userId = GetUserId(Context.User);

        var notification = new Notification
        {
            SenderId = int.Parse(userId),
            ReceiverId = int.Parse(recieverUserId),
            CreatedAt = DateTime.Now,
            Type = notificationType
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var isUserOnline = OnlineUsers.ContainsValue(recieverUserId);

        var userNotification = new UserNotification
        {
            UserId = int.Parse(recieverUserId),
            NotificationId = notification.Id,
            IsRead = isUserOnline,
            ReadAt = isUserOnline ? DateTime.Now : null
        };

        _context.UserNotifications.Add(userNotification);
        await _context.SaveChangesAsync();

        var connectionIds = OnlineUsers.Where(kvp => kvp.Value == recieverUserId).Select(kvp => kvp.Key).ToList();
        await Clients.Clients(connectionIds).SendAsync("ReceiveNotification", notificationType, notificationTitle);
    }

    public async Task AddUserToGroup(int userId, string projectId)
    {
        var groupName = $"Project_{projectId}";
        var result = await _userService.AddUserToNotificationGroup(userId, groupName);

        if (result.IsSuccess && OnlineUsers.ContainsValue(userId.ToString()))
        {
            var connectionId = OnlineUsers.FirstOrDefault(kvp => kvp.Value == userId.ToString()).Key;
            if (connectionId != null)
            {
                await Groups.AddToGroupAsync(connectionId, groupName);
            }
        }
    }

    public async Task RemoveUserFromGroup(int userId, string projectId)
    {
        var groupName = $"Project_{projectId}";
        var result = await _userService.RemoveUserFromNotificationGroup(userId, groupName);

        if (result.IsSuccess && OnlineUsers.ContainsValue(userId.ToString()))
        {
            var connectionId = OnlineUsers.FirstOrDefault(kvp => kvp.Value == userId.ToString()).Key;
            if (connectionId != null)
            {
                await Groups.RemoveFromGroupAsync(connectionId, groupName);
            }
        }
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