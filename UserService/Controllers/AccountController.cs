using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos.Request;
using UserService.Services;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AccountController : ControllerBase
{
    public readonly AccountService _AccountService;
    
    
    public AccountController(AccountService accountService)
    {
        _AccountService = accountService;
    }
    [Authorize]
    [HttpGet("/hello")]
    public string Hello()
    {
        return "hello from user service";
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegistrationDto registrationDto)
    {
        var registrationProcess = await _AccountService.Register(registrationDto);

        return Ok(registrationProcess);
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var loginProcess = await _AccountService.Login(loginDto);

        return Ok(loginProcess);
    }
}