using Common.Dtos;

namespace Common.Contracts;

public record SearchingForCourierEvent //idk what to name tbh
{
    public int OrderId { set; get; }
    
    public string? DeliveryAddress { set; get; }
    
    public string? DeliveryLocation { set; get; }
    
    public string RestaurantName { get; set; }
    
    public string RestaurantAddress { get; set; }
    
    public string RestaurantLocation { get; set; }
    
    public int OrderNumber { get; set; }
    public List<ProductDto> OrderItems { get; set; }
    
} 