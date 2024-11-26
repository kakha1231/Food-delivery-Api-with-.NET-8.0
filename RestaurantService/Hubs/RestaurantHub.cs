using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;
using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using RestaurantService.Services;

namespace RestaurantService.Hubs;

public class RestaurantHub : Hub
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly RestaurantManagementService _restaurantManagementService;
    
    //TODO implement double map to faster up lookup process and change inmemory to redis
    //  (restaurantId -> connectionId) 
    //will not be needed after authorisation
    private static readonly ConcurrentDictionary<string, string> RestaurantConnections = new();

    public RestaurantHub(IPublishEndpoint publishEndpoint, RestaurantManagementService restaurantManagementService)
    {
        _publishEndpoint = publishEndpoint;
        _restaurantManagementService = restaurantManagementService;
    }

    public override async Task OnConnectedAsync()
    {
        // Each client must pass their restaurantId upon connecting
        string restaurantId = Context.GetHttpContext()?.Request.Query["restaurantId"];
        
        if (!string.IsNullOrEmpty(restaurantId))
        {
            RestaurantConnections[restaurantId] = Context.ConnectionId;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var restaurantId = RestaurantConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (restaurantId != null)
        {
            RestaurantConnections.TryRemove(restaurantId, out _);
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Optional: Method for a restaurant to manually register itself if needed
    public Task RegisterRestaurant(string restaurantId)
    {
        RestaurantConnections[restaurantId] = Context.ConnectionId;
        return Task.CompletedTask;
    }
    
    public static string? GetConnectionId(string restaurantId)
    {
        RestaurantConnections.TryGetValue(restaurantId, out string? connectionId);
        return connectionId;
    }
    
    
    public async Task AcceptOrder(int orderId)
    {
        string restaurantId = RestaurantConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        
        
        if (restaurantId != null)
        {
            var restaurant = await _restaurantManagementService.GetRestaurantById(int.Parse(restaurantId));
            
            await _publishEndpoint.Publish(new OrderAcceptedByRestaurantEvent
            {
                OrderId = orderId,
                RestaurantId = int.Parse(restaurantId),
                RestaurantName = restaurant.Name,
                RestaurantLocation = restaurant.Location,
                RestaurantAddress = restaurant.Address,
                Timestamp = default,
            });
        }
    }
    public async Task RejectOrder(int orderId)
    {
        string restaurantId = RestaurantConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

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
        string restaurantId = RestaurantConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        if (restaurantId != null)
        {
            await _publishEndpoint.Publish(new OrderStatusUpdatedEvent
            {
                OrderId = orderId,
                RestaurantId = int.Parse(restaurantId),
                Status = status
            });
        }
        
    }
}
