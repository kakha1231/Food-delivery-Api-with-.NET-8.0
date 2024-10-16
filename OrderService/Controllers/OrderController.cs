using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderManagementService _orderManagementService;

    public OrderController(OrderManagementService orderManagementService)
    {
        _orderManagementService = orderManagementService;
    }


    [Authorize]
    [HttpPost("/create-order")]
    public async Task<Order> CreateOrder(CreateOrderDto createOrderDto)
    {
        var userId = User.Claims.First(u => u.Type == "Id").Value;
        
        return await _orderManagementService.CreateOrder(createOrderDto,userId);
    }
}