using Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestaurantService.Models;

namespace RestaurantService.Dtos.Response;

public class ProductResponseDto
{
    public int Id { set; get; }
    
    public string Name { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public ProductCategory Category { set; get; }
    
    public decimal Price { get; set; }
    
    public string Description { set; get; }

    public static ProductResponseDto FromProduct(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            Description = product.Description
        };
    }
}