using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;

namespace OrderService.Consumers;

public sealed class OrderStatusUpdatedEventConsumer : IConsumer<OrderStatusUpdatedEvent>
{
    private readonly OrderDbContext _orderDbContext;

    public async Task Consume(ConsumeContext<OrderStatusUpdatedEvent> context)
    {
        var order = await _orderDbContext.Orders
            .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
            .FirstOrDefaultAsync();

        if (!Enum.TryParse<OrderStatus>(context.Message.Status, true, out var parsedStatus))
        {
            throw new ArgumentException("Invalid category");
        }
        
        if (order != null)
        {
            order.Status = parsedStatus;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}