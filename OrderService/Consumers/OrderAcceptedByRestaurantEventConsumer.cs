using Common.Contracts;
using Common.Dtos;
using Common.Enums;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OrderService.Entity;
using OrderService.Hubs;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Consumers;

public sealed class OrderAcceptedByRestaurantEventConsumer : IConsumer<OrderAcceptedByRestaurantEvent>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly RedisCacheService _redisCacheService;

    public OrderAcceptedByRestaurantEventConsumer(OrderDbContext orderDbContext, IPublishEndpoint publishEndpoint, IHubContext<OrderHub> hubContext, RedisCacheService redisCacheService)
    {
        _orderDbContext = orderDbContext;
        _publishEndpoint = publishEndpoint;
        _hubContext = hubContext;
        _redisCacheService = redisCacheService;
    }

    public async Task Consume(ConsumeContext<OrderAcceptedByRestaurantEvent> context)
    {
        
        var order = await _redisCacheService.Get<Order>(context.Message.OrderId.ToString());

        if (order == null) 
        {
            order = await _orderDbContext.Orders
                .Where(o => o.Id == context.Message.OrderId && o.RestaurantId == context.Message.RestaurantId)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync();
        }

        if (order != null && order.Status != OrderStatus.Accepted)
        {
            
            await _hubContext.Clients.Group($"OrderGroup-{order.Id}").SendAsync("ReceiveOrderStatus", new
            {
                Status = OrderStatus.Accepted.ToString(),
            });
            
            await _publishEndpoint.Publish(new SearchingForCourierEvent
            {
                OrderId = context.Message.OrderId,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryLocation = order.DeliveryLocation,
                RestaurantName = context.Message.RestaurantName,
                RestaurantAddress = context.Message.RestaurantAddress,
                RestaurantLocation = context.Message.RestaurantLocation,
                OrderNumber = order.OrderNumber,
                OrderItems = order.OrderItems.Select(it => new ProductDto
                {
                    Id = it.ProductId,
                    Name = it.ProductName,
                    Price = it.UnitPrice,
                    Quantity = it.Quantity
                }).ToList(),
            });
            
            order.Status = OrderStatus.Accepted;
            order.UpdatedAt = context.Message.Timestamp;
            
            await _redisCacheService.Update(order.Id.ToString(), order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}