﻿using Microsoft.EntityFrameworkCore;
using Npgsql;
using RestaurantService.Dtos.Request;
using RestaurantService.Dtos.Response;
using RestaurantService.Entity;
using RestaurantService.Models;

namespace RestaurantService.Services;

public class ProductManagementService
{
    private readonly RestaurantDbContext _restaurantDbContext;
    private readonly ILogger<ProductManagementService> _logger;

    public ProductManagementService(RestaurantDbContext restaurantDbContext, ILogger<ProductManagementService> logger)
    {
        _restaurantDbContext = restaurantDbContext;
        _logger = logger;
    }


    public async Task<List<ProductResponseDto>> GetProducts()
    {
        var products = await _restaurantDbContext.Products
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                Description = p.Description
            })
            .ToListAsync();
        return products;
    }

    public async Task<ProductResponseDto> GetProduct(int productId)
    {
        var product = await _restaurantDbContext.Products.FindAsync(productId);

        if (product == null)
        {
            throw new ArgumentException("Product Not Fount");
        }

        return ProductResponseDto.FromProduct(product);
    }
    

    public async Task<ProductResponseDto> AddProduct(AddProductDto productDto, string userId)
    {
        var restaurant = await _restaurantDbContext.Restaurants.FirstOrDefaultAsync(r => r.OwnerId == userId);

        if(restaurant == null)
        {
            throw new ArgumentException("Restaurant Not Found");
        }

        var product = productDto.CreateProduct();

        product.RestaurantId = restaurant.Id;
        product.Restaurant = restaurant;
        
        try
        {
            await _restaurantDbContext.Products.AddAsync(product);
            await _restaurantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed To Add Product For Restaurant {restaurantId}",restaurant.Id);
           
            throw new Exception("Failed To Add Product");
        }
        
        return ProductResponseDto.FromProduct(product);
    }

    
    public async Task<ProductResponseDto> EditProduct(int productId,AddProductDto editProductDto, string userId)
    {
        var restaurant = await _restaurantDbContext.Restaurants
            .Include(r => r.Products)
            .FirstOrDefaultAsync(r => r.OwnerId == userId);

        if (restaurant == null)
        {
            throw new ArgumentException("Restaurant Not Found");
        }
        
        var product =  restaurant.Products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            throw new ArgumentException("Product Not Found or does not belong to your restaurant");
        }
        
        product.Name = editProductDto.Name;
        product.Category = editProductDto.Category;
        product.Price = editProductDto.Price;
        product.Description = editProductDto.Description;
        product.InStock = editProductDto.InStock;

        try
        {
            _restaurantDbContext.Products.Update(product);
            
            await _restaurantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to edit product for restaurant {restaurantId}", restaurant.Id);
            throw new Exception("Failed to edit product");
        }
        
        return ProductResponseDto.FromProduct(product);
    }
    
}