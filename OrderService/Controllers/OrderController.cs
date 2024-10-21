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
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderController(OrderManagementService orderManagementService, IHttpClientFactory httpClientFactory)
    {
        _orderManagementService = orderManagementService;
        _httpClientFactory = httpClientFactory;
    }


    [Authorize]
    [HttpPost("/create-order")]
    public async Task<ActionResult<string>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var userId = User.Claims.First(u => u.Type == "Id").Value;
        
        return Ok(await _orderManagementService.CreateOrder(createOrderDto,userId));
    }


    
}

