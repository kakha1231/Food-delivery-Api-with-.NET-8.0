using Microsoft.EntityFrameworkCore;
using RestaurantService.Models;

namespace RestaurantService.Entity;

public class RestaurantDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { set; get; }
    public DbSet<Product> Products { set; get; }
    
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Restaurant)
            .WithMany(r => r.Products)
            .HasForeignKey(p => p.RestaurantId);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Category)
            .HasConversion<int>();
    }

}