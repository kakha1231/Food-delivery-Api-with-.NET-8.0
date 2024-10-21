using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos;

public class OrderItemDto
{
    [Required]
    public int ProductId { get; set; } 

    [Required]
    public int Quantity { get; set; } 
}