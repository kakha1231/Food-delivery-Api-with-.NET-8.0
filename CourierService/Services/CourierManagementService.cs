using Common.Contracts;
using CourierService.Dtos.Request;
using CourierService.Entity;
using CourierService.Models;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.EntityFrameworkCore;

namespace CourierService.Services;

public class CourierManagementService
{
    private readonly CourierDbContext _courierDbContext;
    private readonly IPublishEndpoint _iPublishEndpoint;

    public CourierManagementService(CourierDbContext courierDbContext, IPublishEndpoint publishEndpoint)
    {
        _courierDbContext = courierDbContext;
        _iPublishEndpoint = publishEndpoint;
    }


    public async Task<Courier> RegisterCourier(CourierRegistrationDto courierRegistrationDto,string userId)
    {
        var courierExists =await _courierDbContext.Couriers.FirstOrDefaultAsync(c => c.UserId == userId);

        if (courierExists != null)
        {
            throw new Exception("You are already a courier");
        }
        
        var courier = courierRegistrationDto.CreateCourier();
        courier.UserId = userId;

        await _courierDbContext.Couriers.AddAsync(courier);
        await _courierDbContext.SaveChangesAsync();

        await _iPublishEndpoint.Publish(new CourierRegisteredEvent
        {
            UserId = userId
        });

        return courier;
    }
}