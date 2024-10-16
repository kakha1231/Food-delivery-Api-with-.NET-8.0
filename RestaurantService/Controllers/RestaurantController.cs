using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Dtos.Request;
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
    
    
    [HttpGet("/hello")]
    public string Hello()
    {
        return "Hello from restaurant";
    }

    [HttpPost("/register-restaurant")]
    [Authorize]
    public async Task<ActionResult<Restaurant>> RegisterRestaurant(RestaurantRegistrationDto registrationDto)
    {

        var userId = User.Claims.First(u => u.Type == "Id").Value;
        
       return Ok(await _restaurantManagementService.RegisterRestaurant(registrationDto,userId));
    }
    
    
}