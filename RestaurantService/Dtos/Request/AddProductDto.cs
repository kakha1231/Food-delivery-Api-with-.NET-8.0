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
        return new Product
        {
            Name = Name,
            Category = Category,
            Price = Price,
            Description = Description,
            InStock = InStock
        };
    }
}