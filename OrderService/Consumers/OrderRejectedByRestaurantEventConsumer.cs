using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;
using OrderService.Hubs;

namespace OrderService.Consumers;

public sealed class OrderRejectedByRestaurantEventConsumer : IConsumer<OrderRejectedByRestaurantEvent>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly IHubContext<OrderHub> _hubContext;
    public OrderRejectedByRestaurantEventConsumer(OrderDbContext orderDbContext, IHubContext<OrderHub> hubContext)
    {
        _orderDbContext = orderDbContext;
        _hubContext = hubContext;
    }
    
    public async Task Consume(ConsumeContext<OrderRejectedByRestaurantEvent> context)
    {
        var order = await _orderDbContext.Orders
            .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
            .FirstOrDefaultAsync();

        if (order != null && order.Status != OrderStatus.Rejected)
        {
            await _hubContext.Clients.Group($"OrderGroup-{order.Id}").SendAsync("ReceiveOrderStatus", new
            {
                Status = OrderStatus.Rejected.ToString(),
            });
            
            order.Status = OrderStatus.Rejected;
            order.UpdatedAt = context.Message.Timestamp;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}