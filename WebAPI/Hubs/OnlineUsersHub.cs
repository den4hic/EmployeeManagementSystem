using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebAPI.Hubs;

[Authorize]
public class OnlineUsersHub : Hub
{
    private static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        string userName = Context.User?.Identity?.Name ?? "Unknown";
        string userId = Context.UserIdentifier ?? Context.ConnectionId;
        OnlineUsers.TryAdd(userName, Context.ConnectionId);
        await Clients.All.SendAsync("UserConnected", userName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string userName = Context.User?.Identity?.Name ?? "Unknown";
        string userId = Context.UserIdentifier ?? Context.ConnectionId;
        OnlineUsers.TryRemove(userName, out _);
        await Clients.All.SendAsync("UserDisconnected", userName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetOnlineUsers()
    {
        await Clients.Caller.SendAsync("OnlineUsers", OnlineUsers.Keys);
    }
}