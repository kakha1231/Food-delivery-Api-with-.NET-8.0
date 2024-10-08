using Microsoft.AspNetCore.Mvc;

namespace RestaurantService.Controllers;

[Route("api/[Controller]")]
[ApiController]

public class RestaurantController : ControllerBase
{
    public RestaurantController()
    {
    }
    
    
    [HttpGet("/hello")]
    public string Hello()
    {
        return "Hello from restaurant";
    }
}