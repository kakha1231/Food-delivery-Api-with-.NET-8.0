using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;

namespace OrderService.Consumers;

public sealed class OrderAcceptedByRestaurantEventConsumer : IConsumer<OrderAcceptedByRestaurantEvent>
{
    private readonly OrderDbContext _orderDbContext;

    public OrderAcceptedByRestaurantEventConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<OrderAcceptedByRestaurantEvent> context)
    {
        var order = await _orderDbContext.Orders
            .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
            .FirstOrDefaultAsync();

        if (order != null && order.Status != OrderStatus.Accepted)
        {
            order.Status = OrderStatus.Accepted;
            order.UpdatedAt = context.Message.Timestamp;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}