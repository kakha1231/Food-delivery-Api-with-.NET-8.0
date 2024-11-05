using CourierService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierService.Entity;

public class CourierDbContext : DbContext
{
    public DbSet<Courier> Couriers { set; get; }
    
    public CourierDbContext(DbContextOptions<CourierDbContext> options) : base(options)
    {
        
    }
    
}