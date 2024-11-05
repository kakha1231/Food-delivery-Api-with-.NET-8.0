using Common.Dtos;

namespace Common.Contracts;

public record OrderCreatedEvent
{
    public int OrderId { get; set; }
    
    public int RestaurantId { get; set; }
    
    public string? Notes { get; set; }
    
    public List<ProductDto> OrderItems { get; set; }
    
    public int OrderNumber { get; set; }
    
}
