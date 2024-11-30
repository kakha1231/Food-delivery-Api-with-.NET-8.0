using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Dtos.Request;
using RestaurantService.Dtos.Response;
using RestaurantService.Models;
using RestaurantService.Services;

namespace RestaurantService.Controllers;

[Route("api/[Controller]")]
[ApiController]

public class RestaurantController : ControllerBase
{
    private readonly RestaurantManagementService _restaurantManagementService;
    public RestaurantController(RestaurantManagementService restaurantManagementService)
    {
        _restaurantManagementService = restaurantManagementService;
    }

    [HttpGet("/restaurants")]
    public async Task<ActionResult<List<RestaurantResponseDto>>> GetRestaurants([FromQuery]List<string> categories)
    {
        return Ok(await _restaurantManagementService.GetRestaurants(categories));
    }

    [HttpGet("/restaurants/{id}")]
    public async Task<ActionResult<RestaurantWithProductsResponseDto>> GetRestaurantById(int id)
    {
        return Ok(await _restaurantManagementService.GetRestaurantById(id));
    }
    
    [HttpPost("/register-restaurant")]
    [Authorize]
    public async Task<ActionResult<Restaurant>> RegisterRestaurant(RestaurantRegistrationDto registrationDto)
    {

        var userId = User.Claims.First(u => u.Type == "Id").Value;
        
       return Ok(await _restaurantManagementService.RegisterRestaurant(registrationDto,userId));
    }
    
    
}