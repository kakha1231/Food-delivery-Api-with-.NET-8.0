﻿using Common.Contracts;
using Common.Dtos;
using Common.Enums;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using OrderService.Dtos;
using OrderService.Entity;
using OrderService.Models;
using StackExchange.Redis;
using Order = OrderService.Models.Order;

namespace OrderService.Services;

public class OrderManagementService
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<OrderManagementService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly RedisCacheService _redisCacheService;

    public OrderManagementService(OrderDbContext orderDbContext, ILogger<OrderManagementService> logger, IHttpClientFactory httpClientFactory, IPublishEndpoint publishEndpoint, RedisCacheService redisCacheService)
    {
        _orderDbContext = orderDbContext;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _publishEndpoint = publishEndpoint;
        _redisCacheService = redisCacheService;
    }

    public async Task<string> CreateOrder(CreateOrderDto createOrderDto, string userId)
    {
        var productValidationResult = await ValidateProducts(createOrderDto);
        
        if (!productValidationResult.IsValid)
        {
            throw new Exception("Invalid order details");
        }
        
        var order = new Order
        {
            CostumerId = userId,
            RestaurantId = createOrderDto.RestaurantId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DeliveryAddress = createOrderDto.DeliveryAddress,
            DeliveryLocation = createOrderDto.DeliveryLocation,
            Notes = createOrderDto.Notes,
            OrderItems = productValidationResult.ValidProducts.Select(pr => new OrderItem
            {
                ProductId = pr.Id,
                ProductName = pr.Name,
                UnitPrice = pr.Price,
                Quantity = pr.Quantity
            }).ToList(),
            TotalAmount = productValidationResult.ValidProducts.Sum(pr => pr.Quantity * pr.Price),
            OrderNumber = new Random().Next(100, 1000)
        };
        
        _orderDbContext.Orders.Add(order);
        
        await _orderDbContext.SaveChangesAsync();
        await _redisCacheService.Set(order.Id.ToString(), order);
        
        await _publishEndpoint.Publish(new OrderCreatedEvent
        {
            OrderId = order.Id,
            RestaurantId = order.RestaurantId,
            Notes = order.Notes,
            OrderItems = productValidationResult.ValidProducts,
            OrderNumber = order.OrderNumber
        });
        return $"order created {order.Id}";
    }


    private async Task<ProductValidationResult> ValidateProducts(CreateOrderDto createOrderDto)
    {
        var httpClient = _httpClientFactory.CreateClient("restaurantServiceClient");
        
         var validationRequestDto = new ProductValidationRequest
         {
             RestaurantId = createOrderDto.RestaurantId,
             Items = createOrderDto.OrderItems.Select(it => new ProductDto
             {
                 Id = it.ProductId,
                 Quantity = it.Quantity
             })
         };
            
         var response = await httpClient.PostAsJsonAsync("/validate", validationRequestDto);
         
         if (!response.IsSuccessStatusCode)
         {
             Console.WriteLine(response.IsSuccessStatusCode);
         }
         
         var validProducts = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

         if (validProducts?.Count != createOrderDto.OrderItems.Count)
         {
             return new ProductValidationResult { IsValid = false };
         }
         return new ProductValidationResult { IsValid = true, ValidProducts = validProducts };
    }
}


