using Common.Contracts;
using Common.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Dtos.Request;
using RestaurantService.Dtos.Response;
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


    public async Task<List<RestaurantResponseDto>> GetRestaurants(List<string>? categories = null, int page = 1)
    {
        
        var parsedCategories = categories?.Where(category => 
                Enum.TryParse<ProductCategory>(category, true, out _))
            .Select(category => Enum.Parse<ProductCategory>(category, true))
            .ToList();
        
        var query = _restaurantDbContext.Restaurants.
            Skip((page - 1) * 20)
            .Take(20)
            .AsQueryable();

        if (parsedCategories != null && parsedCategories.Any())
        {
            query = query.Where(r => r.Products.Any(p => parsedCategories.Contains(p.Category)));
        }

        var restaurants =  await query.Select(r => new RestaurantResponseDto 
        {
            Id = r.Id,
            Name = r.Name,
            Type = r.Type,
            Country = r.Country,
            City = r.City,
            PostCode = r.PostCode,
            Address = r.Address,
            Location = r.Location,
            PhoneNumber = r.PhoneNumber,
            Email = r.Email
        }).ToListAsync();
        
        return restaurants;
    }
    
    public async Task<RestaurantWithProductsResponseDto> GetRestaurantById(int id)
    {
        var restaurant =  await _restaurantDbContext.Restaurants.Include(r => r.Products).
            FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
        {
            throw new ArgumentException("Restaurant not found");
        }

        return RestaurantWithProductsResponseDto.FromRestaurant(restaurant);
    }
    
    public async Task<RestaurantResponseDto> RegisterRestaurant(RestaurantRegistrationDto registrationDto,string userId)
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
        
        return RestaurantResponseDto.FromRestaurant(restaurant);
    }
}