using Common.Dtos;

namespace OrderService.Dtos;

public class ProductValidationRequest
{
    public int RestaurantId { get; set; }
    
    public IEnumerable<ProductDto>? Items { set; get; }
    
}