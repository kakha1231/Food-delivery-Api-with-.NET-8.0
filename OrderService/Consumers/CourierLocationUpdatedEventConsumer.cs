using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using OrderService.Hubs;

namespace OrderService.Consumers;

public class CourierLocationUpdatedEventConsumer : IConsumer<CourierLocationUpdatedEvent>
{
    private readonly IHubContext<OrderHub> _hubContext;

    public CourierLocationUpdatedEventConsumer(IHubContext<OrderHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<CourierLocationUpdatedEvent> context)
    {
        await _hubContext.Clients.Group($"OrderGroup-{context.Message.OrderId}").SendAsync("ReceiveCourierLocation", 
            new {
                Latitude = context.Message.Latitude,
                Longitude = context.Message.Longitude });
    }
}