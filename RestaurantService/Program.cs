using System.Text;
using System.Text.Json.Serialization;
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
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Optional: Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<RestaurantManagementService>();
builder.Services.AddScoped<ProductManagementService>();

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:JwtSecret").Get<string>();

builder.Services.AddJwtAuthentication(jwtIssuer, jwtKey);

builder.Services.AddCors(options =>  //for testing purposes i'm using cors and mock frontend to test websockets
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

    busConfigurator.AddConsumer<OrderCreatedEventConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["MessageBroker:Host"], h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceDiscovery(op => op.UseConsul());;

builder.Services.AddHttpClient().AddServiceDiscovery(op => op.UseConsul());

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

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