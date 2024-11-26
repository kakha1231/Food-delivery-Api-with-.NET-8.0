using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace CourierService.Hubs;

public class CourierHub : Hub
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CourierHub(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public override async Task OnConnectedAsync()
    {
            await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableCouriers");
    }
    
    public async Task AcceptOrder(string orderId, int courierId)
    {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AvailableCouriers");

           await _publishEndpoint.Publish(new OrderAcceptedByCourierEvent
            {
                OrderId = orderId,
                CourierId = courierId,
            });
    }
    
    public async Task UpdateLocation(int orderId, double latitude, double longitude)
    {
        await _publishEndpoint.Publish(new CourierLocationUpdatedEvent
        {
            OrderId = orderId,
            Latitude = latitude,
            Longitude = longitude
        });
    }
    
    public async Task SendOrderStatusUpdate(string orderId, string status)
    {
        await _publishEndpoint.Publish(new OrderStatusUpdatedEvent
        {
            OrderId = int.Parse(orderId),
            Status = status
        });
    }
    
    public async Task SetCourierAvailable()
    {
            await Groups.AddToGroupAsync(Context.ConnectionId, "AvailableCouriers");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AvailableCouriers");
        await base.OnDisconnectedAsync(exception);
    }
}