using System.Text.Json.Serialization;
using Common.Configuration;
using Common.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using OrderService.Consumers;
using OrderService.Entity;
using OrderService.Hubs;
using OrderService.Services;
using StackExchange.Redis;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "Orders_";
});

builder.Services.AddScoped<OrderManagementService>();
builder.Services.AddScoped<RedisCacheService>();

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:JwtSecret").Get<string>();

builder.Services.AddJwtAuthentication(jwtIssuer, jwtKey);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .WithOrigins(
                    "http://localhost:5500",
                    "http://localhost:5501",
                    "http://localhost:5502",
                    "http://localhost:5503" 
                )
                .AllowAnyMethod() 
                .AllowAnyHeader() 
                .AllowCredentials();
        });
});


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderAcceptedByRestaurantEventConsumer>();
    busConfigurator.AddConsumer<OrderRejectedByRestaurantEventConsumer>();
    busConfigurator.AddConsumer<OrderStatusUpdatedEventConsumer>();
    busConfigurator.AddConsumer<CourierLocationUpdatedEventConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["MessageBroker:Host"], h =>
        {
            h.Username(builder.Configuration["MessageBroker: Username"]);
            h.Password(builder.Configuration["MessageBroker: Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceDiscovery(op => op.UseConsul());

builder.Services.AddHttpClient("userServiceClient", client =>
{
    client.BaseAddress = new Uri("http://user-service");
}).AddServiceDiscovery();

builder.Services.AddHttpClient("productServiceClient", client =>
{
    client.BaseAddress = new Uri("http://restaurant-service");
}).AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Optional: for API docs in development
    app.UseSwaggerUI(); // Optional: for Swagger UI
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers(); // Map controller endpoints

app.MapHub<OrderHub>("/orderHub");

app.MapGet("/health", () => "healthy");

app.Run();