using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using UserService.Models;

namespace UserService.Consumers;

public sealed class CourierRegisteredEventConsumer : IConsumer<CourierRegisteredEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RestaurantRegisteredEventConsumer> _logger;

    public CourierRegisteredEventConsumer(UserManager<User> userManager, ILogger<RestaurantRegisteredEventConsumer> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CourierRegisteredEvent> context)
    {
        var user = await _userManager.FindByIdAsync(context.Message.UserId);

        if (user == null)
        {
            throw new ArgumentException("User Not Found");
        }

        var roleAssignmentResult =  await _userManager.AddToRoleAsync(user, "Courier");
       
        if (!roleAssignmentResult.Succeeded)
        {
            var roleErrorMessage = string.Join("\n", roleAssignmentResult.Errors.Select(e => e.Description));
           
            _logger.LogError("Error assigning role: {errors}", roleErrorMessage);
          
            throw new InvalidOperationException("Failed To Assign Role.");
        }
    }
}