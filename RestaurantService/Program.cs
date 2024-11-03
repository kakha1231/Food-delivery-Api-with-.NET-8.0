using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantService.Entity;
using RestaurantService.Services;
using Common.Configuration;
using RestaurantService.Consumers;
using RestaurantService.Hubs;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Optional: Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<RestaurantManagementService>();
builder.Services.AddScoped<ProductManagementService>();

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:JwtSecret").Get<string>();

builder.Services.AddJwtAuthentication(jwtIssuer, jwtKey);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .WithOrigins(
                    "http://localhost:5500"   // Live Server default
                )
                .AllowAnyMethod() // Allow any HTTP method (GET, POST, etc.)
                .AllowAnyHeader() // Allow any HTTP header
                .AllowCredentials();
        });
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderCreatedEventConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(("rabbitmq"), h =>
        {
            h.Username(builder.Configuration["MessageBroker: Username"]);
            h.Password(builder.Configuration["MessageBroker: Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceDiscovery(op => op.UseConsul());;

builder.Services.AddHttpClient().AddServiceDiscovery(op => op.UseConsul());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.MapHub<RestaurantHub>("/restaurantHub");

app.MapGet("/health", () => "healthy");

app.Run();