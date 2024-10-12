using Common.Contracts;
using MassTransit;

namespace UserService.Consumers;

public class RestaurantRegisteredEventConsumer : IConsumer<RestaurantRegisteredEvent>
{
    public Task Consume(ConsumeContext<RestaurantRegisteredEvent> context)
    {
        Console.WriteLine("we received RestaurantRegisteredEvent" + context.Message.UserId);
        
        return Task.CompletedTask;
    }
    
}