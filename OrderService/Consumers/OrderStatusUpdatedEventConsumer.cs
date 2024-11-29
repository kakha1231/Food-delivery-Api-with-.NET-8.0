using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;
using OrderService.Hubs;

namespace OrderService.Consumers;

public sealed class OrderStatusUpdatedEventConsumer : IConsumer<OrderStatusUpdatedEvent>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrderStatusUpdatedEventConsumer(OrderDbContext orderDbContext, IHubContext<OrderHub> hubContext)
    {
        _orderDbContext = orderDbContext;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<OrderStatusUpdatedEvent> context)
    {
        var order = await _orderDbContext.Orders.FindAsync(context.Message.OrderId);

        if (!Enum.TryParse<OrderStatus>(context.Message.Status, true, out var parsedStatus))
        {
            throw new ArgumentException("Invalid status");
        }
        
        if (order != null)
        {
            await _hubContext.Clients.Group($"OrderGroup-{order.Id}").SendAsync("ReceiveOrderStatus", new
            {
                Status = context.Message.Status,
            });
            
            order.Status = parsedStatus;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}