using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;
using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace RestaurantService.Hubs;

public class RestaurantHub : Hub
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    //TODO implement double map to faster up lookup process and change inmemory to redis
    //  (restaurantId -> connectionId) 
    private static readonly ConcurrentDictionary<string, string> Connections = new();

    public RestaurantHub(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public override async Task OnConnectedAsync()
    {
        // Each client must pass their restaurantId upon connecting
        string restaurantId = Context.GetHttpContext()?.Request.Query["restaurantId"];
        
        if (!string.IsNullOrEmpty(restaurantId))
        {
            Connections[restaurantId] = Context.ConnectionId;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var restaurantId = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (restaurantId != null)
        {
            Connections.TryRemove(restaurantId, out _);
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Optional: Method for a restaurant to manually register itself if needed
    public Task RegisterRestaurant(string restaurantId)
    {
        Connections[restaurantId] = Context.ConnectionId;
        return Task.CompletedTask;
    }
    
    public static string? GetConnectionId(string restaurantId)
    {
        Connections.TryGetValue(restaurantId, out string? connectionId);
        return connectionId;
    }
    
    
    public async Task AcceptOrder(int orderId)
    {
        string restaurantId = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        
        if (restaurantId != null)
        {
            await _publishEndpoint.Publish(new OrderAcceptedByRestaurantEvent
            {
                OrderId = orderId,
                RestaurantId = int.Parse(restaurantId)
            });
        }
    }
    public async Task RejectOrder(int orderId)
    {
        string restaurantId = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        if (restaurantId != null)
        {
            await _publishEndpoint.Publish(new OrderRejectedByRestaurantEvent
            {
                OrderId = orderId,
                RestaurantId = int.Parse(restaurantId)
            });
        }
    }

    public async Task UpdateOrderStatus(int orderId, string status)
    {
        Console.WriteLine($"status updated {status}");
        string restaurantId = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        if (restaurantId != null)
        {
            Console.WriteLine($"status updated {status} part 2");
            await _publishEndpoint.Publish(new OrderStatusUpdatedEvent
            {
                OrderId = orderId,
                RestaurantId = int.Parse(restaurantId),
                Status = status
            });
        }
        
    }
}
