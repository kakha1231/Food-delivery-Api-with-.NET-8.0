using Common.Contracts;
using CourierService.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace CourierService.Consumers;

public class SearchingForCourierEventConsumer : IConsumer<SearchingForCourierEvent>
{
    private readonly IHubContext<CourierHub> _hubContext;

    public SearchingForCourierEventConsumer(IHubContext<CourierHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<SearchingForCourierEvent> context)
    {
        await _hubContext.Clients.Group("AvailableCouriers")
           .SendAsync("ReceiveOrderNotification", new
           {
               OrderId = context.Message.OrderId,
               DeliveryAddress = context.Message.DeliveryAddress,
               DeliveryLocation = context.Message.DeliveryLocation,
               RestaurantName = context.Message.RestaurantName,
               RestaurantAddress = context.Message.RestaurantAddress,
               RestaurantLocation = context.Message.RestaurantLocation,
               OrderItems = context.Message.OrderItems,
               OrderNumber = context.Message.OrderNumber,
               CreatedAt = DateTime.UtcNow
           });
    }
}