using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Dtos.Request;
using UserService.Entity;
using UserService.Jwt;
using UserService.Models;

namespace UserService.Services;

public class AccountService
{
    private readonly UserDbContext _userDbContext;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountService> _logger;
    private readonly JwtService _jwtService;
    
    public AccountService(UserManager<User> userManager, UserDbContext userDbContext, ILogger<AccountService> logger, JwtService jwtService)
    {
        _userManager = userManager;
        _userDbContext = userDbContext;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<string> Register(RegistrationDto registrationDto)
    {
        _logger.LogInformation("Started Registration Process for Email: {username}", registrationDto.Email);

        var checkEmail = await _userManager.FindByEmailAsync(registrationDto.Email);
       
        if (checkEmail != null)
        {
            return "Email already exists";
        }
        
        if (await CheckPhoneNumber(registrationDto.PhoneNumber))
        {
            return "Phone number already exists";
        }
        
        var userToRegister = registrationDto.CreateUser();
        
        var createProcess = await _userManager.CreateAsync(userToRegister, registrationDto.Password);
       
        if (!createProcess.Succeeded)
        {
            var errorMessage = string.Join("\n", createProcess.Errors.Select(e => e.Description));
          
            _logger.LogError("Error creating user: {errors}", errorMessage);
            
            return errorMessage;
        }
        
        var roleAssignmentResult = await _userManager.AddToRoleAsync(userToRegister, "User");
       
        if (!roleAssignmentResult.Succeeded)
        {
            var roleErrorMessage = string.Join("\n", roleAssignmentResult.Errors.Select(e => e.Description));
           
            _logger.LogError("Error assigning role: {errors}", roleErrorMessage);
            
            return roleErrorMessage;
        }
       
        return "User registered successfully";
    }
    


    public async Task<string> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (await _userManager.FindByEmailAsync(loginDto.Email) == null)
        {
            return "Incorrect email or password";
        }

        var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!checkPassword)
        { 
            return  "Incorrect email or password";
        }

        var jwt = _jwtService.CreateJwt(user);

        return jwt;
    }

    //TODO Mail confirmation method
    private async Task<string> ConfirmEmail(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user.EmailConfirmed)
        {
            return "email is already confirmed";
        }

        var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultProvider);

        return "not yet developed";
    }

    private async Task<bool> CheckPhoneNumber(string phoneNumber)
    {
        var checkResult = await _userDbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

        if (checkResult == null)
        {
            return false;
        }

        return true;
    }
    
}