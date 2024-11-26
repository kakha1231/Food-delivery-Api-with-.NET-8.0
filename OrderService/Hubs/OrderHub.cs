using Common.Enums;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace OrderService.Hubs;

public class OrderHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        string orderId = Context.GetHttpContext()?.Request.Query["orderId"];
        
        if (!string.IsNullOrEmpty(orderId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"OrderGroup-{orderId}");
        }
        await base.OnConnectedAsync();
    }
    

 public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string orderId = Context.GetHttpContext()?.Request.Query["orderId"];

        if (!string.IsNullOrEmpty(orderId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"OrderGroup-{orderId}");
        }
        await base.OnDisconnectedAsync(exception);
    }

    
}