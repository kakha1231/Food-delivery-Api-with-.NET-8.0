using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;
using OrderService.Hubs;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Consumers;

public sealed class OrderRejectedByRestaurantEventConsumer : IConsumer<OrderRejectedByRestaurantEvent>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly RedisCacheService _redisCacheService;
    public OrderRejectedByRestaurantEventConsumer(OrderDbContext orderDbContext, IHubContext<OrderHub> hubContext, RedisCacheService redisCacheService)
    {
        _orderDbContext = orderDbContext;
        _hubContext = hubContext;
        _redisCacheService = redisCacheService;
    }
    
    public async Task Consume(ConsumeContext<OrderRejectedByRestaurantEvent> context)
    {
        var order = await _redisCacheService.Get<Order>(context.Message.OrderId.ToString());

        if (order == null) 
        {
            order = await _orderDbContext.Orders
                .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync();
        }

        if (order != null && order.Status != OrderStatus.Rejected)
        {
            await _hubContext.Clients.Group($"OrderGroup-{order.Id}").SendAsync("ReceiveOrderStatus", new
            {
                Status = OrderStatus.Rejected.ToString(),
            });
            
            order.Status = OrderStatus.Rejected;
            order.UpdatedAt = context.Message.Timestamp;
            
            await _redisCacheService.Remove(order.Id.ToString());
            await _orderDbContext.SaveChangesAsync();
        }
    }
}