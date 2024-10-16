using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Dtos;

public class CreateOrderDto
{
    [Required]
    public int RestaurantId { set; get; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public string DeliveryAddress { set; get; }
    [Required]
    public string DeliveryLocation { set; get; }
    
    public string Notes { set; get; }
    [Required]
    public ICollection<OrderItemDto> OrderItems { set; get; }

    public Order CreateOrder()
    {
        return new Order
        {
            RestaurantId = RestaurantId,
            CreatedAt = CreatedAt,
            DeliveryAddress = DeliveryAddress,
            DeliveryLocation = DeliveryLocation,
            Notes = Notes,
            OrderItems = OrderItems.Select(items => new OrderItem
            {
                ProductId = items.ProductId,
                ProductName = items.ProductName,
                UnitPrice = items.UnitPrice,
                Quantity = items.Quantity
            }).ToList()
        };
    }
}