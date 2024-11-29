using Common.Configuration;
using Common.Contracts;
using CourierService.Consumers;
using CourierService.Entity;
using CourierService.Hubs;
using CourierService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Register controllers
builder.Services.AddEndpointsApiExplorer(); // For Swagger or API documentation, optional
builder.Services.AddSwaggerGen(); // Optional for Swagger UI
builder.Services.AddSignalR();

builder.Services.AddDbContext<CourierDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<CourierManagementService>();

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
                    "http://localhost:5503", 
                    "http://localhost:5504", 
                    "http://localhost:5505"
                ) 
                .AllowAnyMethod() 
                .AllowAnyHeader() 
                .AllowCredentials();
        });
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<SearchingForCourierEventConsumer>();
    
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Optional: for API docs in development
    app.UseSwaggerUI(); // Optional: for Swagger UI
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<CourierHub>("/courierHub");

app.MapControllers(); 

app.Run();