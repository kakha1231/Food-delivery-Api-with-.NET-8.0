using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;

namespace OrderService.Consumers;

public class OrderRejectedByRestaurantEventConsumer : IConsumer<OrderRejectedByRestaurantEvent>
{
    private readonly OrderDbContext _orderDbContext;

    public OrderRejectedByRestaurantEventConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }
    
    public async Task Consume(ConsumeContext<OrderRejectedByRestaurantEvent> context)
    {
        var order = await _orderDbContext.Orders
            .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
            .FirstOrDefaultAsync();

        if (order != null && order.Status != OrderStatus.Rejected)
        {
            order.Status = OrderStatus.Rejected;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}