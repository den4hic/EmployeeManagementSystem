using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebAPI.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private static readonly Dictionary<string, string> OnlineUsers = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        string userId = GetUserId(Context.User);
        string connectionId = Context.ConnectionId;

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
        string connectionId = Context.ConnectionId;

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

    public async Task SendNotification(string userId, NotificationType notificationType)
    {
        var connectionIds = OnlineUsers.Where(kvp => kvp.Value == userId).Select(kvp => kvp.Key).ToList();
        await Clients.Clients(connectionIds).SendAsync("ReceiveNotification", notificationType);
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