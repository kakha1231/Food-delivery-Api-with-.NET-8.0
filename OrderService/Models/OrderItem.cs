using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public int? OrderId { get; set; } 
    [JsonIgnore]
    public Order? Order { set; get; }
}