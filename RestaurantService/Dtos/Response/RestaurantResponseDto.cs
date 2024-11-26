using RestaurantService.Models;

namespace RestaurantService.Dtos.Response;

public class RestaurantResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { set; get; }
    public string Country { get; set; }
    public string City { set; get; }
    public string PostCode { set; get; }
    public string Address { get; set; }
    public string? Location { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { set; get; }

    public static RestaurantResponseDto FromRestaurant(Restaurant restaurant)
    {
        return new RestaurantResponseDto
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
            Email = restaurant.Email
        };
    }
}