﻿using Common.Enums;

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
