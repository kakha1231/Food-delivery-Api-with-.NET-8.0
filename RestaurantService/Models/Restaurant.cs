namespace RestaurantService.Models;

public class Restaurant
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
    
    public string OwnerId { set; get; }
    public ICollection<Product> Products { get; set; }
}

