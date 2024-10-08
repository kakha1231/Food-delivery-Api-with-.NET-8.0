using Microsoft.AspNetCore.Identity;

namespace UserService.Models;

public class User : IdentityUser
{
    public string FirstName { set; get; }
    public string Lastname { set; get; }
    
}