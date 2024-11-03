using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace OrderService.Models;

public class Order
{
    [Key]
    public int Id { set; get; }
    
    public required string CostumerId { set; get; }
    public int RestaurantId { set; get; }
    
    public int? CourierId { set; get; }

    public OrderStatus Status { set; get; }
    
    public decimal? TotalAmount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string? DeliveryAddress { set; get; }
    
    public string? DeliveryLocation { set; get; }
    
    //public decimal DeliveryFee { set; get; } will be implemented later
    
    public string? Notes { set; get; }
    
    public int OrderNumber { set; get; }

    public ICollection<OrderItem> OrderItems { set; get; } = new List<OrderItem>();
}
