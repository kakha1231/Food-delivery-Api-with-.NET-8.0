using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using UserService.Models;

namespace UserService.Consumers;

public sealed class RestaurantRegisteredEventConsumer : IConsumer<RestaurantRegisteredEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RestaurantRegisteredEventConsumer> _logger;

    public RestaurantRegisteredEventConsumer(UserManager<User> userManager, ILogger<RestaurantRegisteredEventConsumer> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<RestaurantRegisteredEvent> context)
    {
        var user = await _userManager.FindByIdAsync(context.Message.UserId);

        if (user == null)
        {
            throw new ArgumentException("User Not Found");
        }

       var roleAssignmentResult =  await _userManager.AddToRoleAsync(user, "RestaurantOwner");
       
       if (!roleAssignmentResult.Succeeded)
       {
           var roleErrorMessage = string.Join("\n", roleAssignmentResult.Errors.Select(e => e.Description));
           
           _logger.LogError("Error assigning role: {errors}", roleErrorMessage);
          
           throw new InvalidOperationException("Failed To Assign Role.");
       }
       
    }
    
}