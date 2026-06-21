using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagement.RazorPages.Hubs;

public class NewsHub : Hub
{
    public async Task JoinNewsGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveNewsGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendNewsUpdate(string message, string newsId, string action)
    {
        await Clients.Group("NewsUpdates").SendAsync("ReceiveNewsUpdate", message, newsId, action);
    }
}
