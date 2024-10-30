namespace RestaurantService.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public ProductCategory Category { set; get; }
    
    public decimal Price { get; set; }
    
    public string Description { set; get; }
    
    public bool InStock { set; get; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
}


public enum ProductCategory
{
    Alcohol,
    American,
    Asian,
    BakeryAndPastry,
    Beer,
    Breakfast,
    Burgers,
    Chicken,
    Desserts,
    Drinks,
    European,
    FastFood,
    Georgian,
    Grill,
    Healthy,
    HotDogs,
    IceCream,
    Indian,
    International,
    Italian,
    Mexican,
    Pasta,
    Pizza,
    Salads,
    Sandwich,
    Shawarma,
    Soup,
    Sushi,
    Sweets,
    TeaCoffee,
    Vegetarian,
    Wine,
}