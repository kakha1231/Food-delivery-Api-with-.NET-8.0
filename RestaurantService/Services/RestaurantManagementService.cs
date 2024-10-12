using Common.Contracts;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Authorization;
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
        var restaurant = registrationDto.CreateRestaurant();
        restaurant.OwnerId = userId;
        try
        {
            await _restaurantDbContext.Restaurants.AddAsync(restaurant);
            await _restaurantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message,"Error While Adding Restaurant");
            throw new Exception(ex.Message);
        }

        await _publishEndpoint.Publish(new RestaurantRegisteredEvent
        {
            UserId = userId
        });
        return restaurant;
    }
}