using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace RestaurantService.Hubs;

public class RestaurantHub : Hub
{
    // Store restaurant connections in memory (restaurantId -> connectionId)
    private static readonly ConcurrentDictionary<string, string> _connections = new();

    public override async Task OnConnectedAsync()
    {
        // Each client must pass their restaurantId upon connecting
        string restaurantId = Context.GetHttpContext()?.Request.Query["restaurantId"];
        
        if (!string.IsNullOrEmpty(restaurantId))
        {
            _connections[restaurantId] = Context.ConnectionId;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Remove restaurant from connection list on disconnect
        string restaurantId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (restaurantId != null)
        {
            _connections.TryRemove(restaurantId, out _);
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Optional: Method for a restaurant to manually register itself if needed
    public Task RegisterRestaurant(string restaurantId)
    {
        _connections[restaurantId] = Context.ConnectionId;
        return Task.CompletedTask;
    }

    // Get the connection ID for a specific restaurant
    public static string? GetConnectionId(string restaurantId)
    {
        _connections.TryGetValue(restaurantId, out string? connectionId);
        return connectionId;
    }
}
