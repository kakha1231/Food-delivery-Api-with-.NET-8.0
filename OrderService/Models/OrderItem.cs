using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public sealed class OrderItem
{
    [Key]
    public int Id { get; set; } 
    
    public int ProductId { get; set; } 
    
    public string? ProductName { get; set; } 
    public decimal UnitPrice { get; set; } 
    public int Quantity { get; set; } 
    private decimal TotalPrice => UnitPrice * Quantity;
    
    public int? OrderId { get; set; } 
    public Order? Order { set; get; }
}