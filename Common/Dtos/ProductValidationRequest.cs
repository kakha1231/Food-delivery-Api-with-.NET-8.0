using Common.Dtos;

namespace Common.Dtos;

public record ProductValidationRequest
{
    public int RestaurantId { get; set; }
    
    public IEnumerable<ProductDto>? Items { set; get; }
    
}