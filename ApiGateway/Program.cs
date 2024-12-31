using ApiGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpClient();

builder.Configuration.AddJsonFile("Ocelot.json",optional: false, reloadOnChange: true);

Console.WriteLine(File.ReadAllText("Ocelot.json"));

builder.Services.AddOcelot()
    .AddConsul<MyConsulServiceBuilder>();

var app = builder.Build();

app.UseAuthorization();

app.UseOcelot().Wait();

app.Run();