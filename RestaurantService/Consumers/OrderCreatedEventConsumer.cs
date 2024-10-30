using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using RestaurantService.Hubs;

namespace RestaurantService.Consumers;

public sealed class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IHubContext<RestaurantHub> _hubContext;

    public OrderCreatedEventConsumer(IHubContext<RestaurantHub> hubContext)
    {
        _hubContext = hubContext;
    }

    
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        Console.WriteLine("Received message about creating order");
        Console.WriteLine(context.Message.OrderId);
        Console.WriteLine(context.Message.Notes);

        string? connectionId = RestaurantHub.GetConnectionId(context.Message.RestaurantId.ToString());
        
        if (connectionId != null)
        {
            // Send notification directly to the specific connection
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveOrderNotification", new
                {
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    Notes = context.Message.Notes,
                    OrderNumber = context.Message.OrderNumber,
                    CreatedAt = DateTime.UtcNow
                });
        }
    }
}