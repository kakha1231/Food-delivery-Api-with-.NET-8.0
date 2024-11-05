using Common.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Dtos.Request;
using RestaurantService.Entity;
using RestaurantService.Models;

namespace RestaurantService.Services;

public class RestaurantManagementService
{
    private readonly RestaurantDbContext _restaurantDbContext;
    private readonly ILogger<RestaurantManagementService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public RestaurantManagementService(RestaurantDbContext restaurantDbContext, ILogger<RestaurantManagementService> logger, IPublishEndpoint publishEndpoint)
    {
        _restaurantDbContext = restaurantDbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }


    public async Task<Restaurant> RegisterRestaurant(RestaurantRegistrationDto registrationDto,string userId)
    {
        var restaurantExists =await _restaurantDbContext.Restaurants.FirstOrDefaultAsync(r => r.OwnerId == userId);

        if (restaurantExists != null)
        {
            throw new Exception("You have already registered restaurant");
        }
        
        var restaurant = registrationDto.CreateRestaurant();
        restaurant.OwnerId = userId;
        
        await _restaurantDbContext.Restaurants.AddAsync(restaurant);
        await _restaurantDbContext.SaveChangesAsync();
        
        await _publishEndpoint.Publish(new RestaurantRegisteredEvent
        {
            UserId = userId
        });
        
        return restaurant;
    }
}