using Common.Dtos;

namespace Common.Library;

public class ProductValidationResult
{
    public bool IsValid { get; set; }
    public List<ProductDto>? ValidProducts { get; set; }
}