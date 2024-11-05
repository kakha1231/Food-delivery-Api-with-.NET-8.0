using CourierService.Dtos.Request;
using CourierService.Models;
using CourierService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierService.Controllers;

[ApiController]
public class CourierController : ControllerBase
{
    private readonly CourierManagementService _courierManagementService;

    public CourierController(CourierManagementService courierManagementService)
    {
        _courierManagementService = courierManagementService;
    }
    
    [Authorize]
    [HttpPost("/Register-Courier")]
    public async Task<ActionResult<Courier>> RegisterCourier(CourierRegistrationDto courierRegistrationDto)
    {
        var userId = User.Claims.First(u => u.Type == "Id").Value;

        return Ok(await _courierManagementService.RegisterCourier(courierRegistrationDto, userId));
    }
    
}