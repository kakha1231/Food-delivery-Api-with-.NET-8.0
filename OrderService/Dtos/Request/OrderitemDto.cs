using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos;

public class OrderItemDto
{
    [Required]
    public int ProductId { get; set; } 
    [Required]
    public string ProductName { get; set; } 
    [Required]
    public decimal UnitPrice { get; set; } 
    [Required]
    public int Quantity { get; set; } 
}