using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Dtos;

public class CreateOrderDto
{
    [Required]
    public int RestaurantId { set; get; }
    
    [Required]
    public string DeliveryAddress { set; get; }
    [Required]
    public string DeliveryLocation { set; get; }
    
    public string Notes { set; get; }
    
    public ICollection<OrderItemDto> OrderItems { set; get; }
    
}