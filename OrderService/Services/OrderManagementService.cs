using OrderService.Dtos;
using OrderService.Entity;
using OrderService.Models;

namespace OrderService.Services;

public class OrderManagementService
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<OrderManagementService> _logger;

    public OrderManagementService(OrderDbContext orderDbContext, ILogger<OrderManagementService> logger)
    {
        _orderDbContext = orderDbContext;
        _logger = logger;
    }

    public async Task<Order> CreateOrder(CreateOrderDto createOrderDto, string userId)
    {
        var order = createOrderDto.CreateOrder();
        order.CostumerId = userId;
        
         await _orderDbContext.Orders.AddAsync(order);
         await _orderDbContext.SaveChangesAsync();
         
         return order;
    }
    
}