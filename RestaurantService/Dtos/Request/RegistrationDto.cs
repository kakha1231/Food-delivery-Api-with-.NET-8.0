using System.ComponentModel.DataAnnotations;
using RestaurantService.Models;

namespace RestaurantService.Dtos.Request;


public class RegistrationDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Type { set; get; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string City { set; get; }
    [Required]
    public string PostCode { set; get; }
    [Required]
    public string Address { get; set; }
    public string? Location { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    [EmailAddress]
    public string Email { set; get; }

    public Restaurant CreateRestaurant()
    {
        return new Restaurant()
        {
            Name = Name,
            Type = Type,
            Country = Country,
            City = City,
            PostCode = PostCode,
            Address = Address,
            Location = Location,
            PhoneNumber = PhoneNumber,
            Email = Email
        };
    }
}