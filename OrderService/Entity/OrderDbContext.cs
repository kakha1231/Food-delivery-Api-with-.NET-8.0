using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Entity;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders;
    public DbSet<OrderItem> OrderItems;
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderItem>()
            .HasOne(it => it.Order)
            .WithMany(ord => ord.OrderItems)
            .HasForeignKey(it => it.OrderId);
    }
    
}