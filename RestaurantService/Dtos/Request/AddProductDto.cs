using Common.Enums;
using RestaurantService.Models;

namespace RestaurantService.Dtos.Request;

public class AddProductDto
{
    public string Name { get; set; }
    
    public string Category { set; get; }
    
    public decimal Price { get; set; }
    
    public string Description { set; get; }

    public bool InStock { set; get; } = true;

    public Product CreateProduct()
    {
        if (!Enum.TryParse<ProductCategory>(Category, true, out var parsedCategory))
        {
            throw new ArgumentException("Invalid category");
        }
        
        return new Product
        {
            Name = Name,
            Category = parsedCategory,
            Price = Price,
            Description = Description,
            InStock = InStock
        };
    }
}