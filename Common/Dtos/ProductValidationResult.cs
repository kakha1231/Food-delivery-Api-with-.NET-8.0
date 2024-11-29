using Common.Dtos;

namespace Common.Dtos;

public record ProductValidationResult
{
    public bool IsValid { get; set; }
    public List<ProductDto>? ValidProducts { get; set; }
}