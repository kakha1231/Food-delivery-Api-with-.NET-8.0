using RestaurantService.Dtos.Response;
using RestaurantService.Models;

public class RestaurantWithProductsResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Address { get; set; }
    public string? Location { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<ProductResponseDto> Products { get; set; } = new();

    public static RestaurantWithProductsResponseDto FromRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new ArgumentNullException(nameof(restaurant));

        return new RestaurantWithProductsResponseDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Type = restaurant.Type,
            Country = restaurant.Country,
            City = restaurant.City,
            PostCode = restaurant.PostCode,
            Address = restaurant.Address,
            Location = restaurant.Location,
            PhoneNumber = restaurant.PhoneNumber,
            Email = restaurant.Email,
            Products = restaurant.Products.Select(ProductResponseDto.FromProduct).ToList()
        };
    }
}