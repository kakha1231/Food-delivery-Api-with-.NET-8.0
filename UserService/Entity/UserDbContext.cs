using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Entity;

public class UserDbContext : IdentityDbContext<User>
{
    
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        
    }
}