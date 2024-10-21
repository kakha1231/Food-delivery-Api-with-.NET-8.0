using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Entity;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { set; get; }
    public DbSet<OrderItem> OrderItems { set; get; }
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId) 
            .OnDelete(DeleteBehavior.Cascade); 
    }
    
}