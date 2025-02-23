using AuthService.API;
using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using BuildingBlocks.Messaging.MassTransit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.InitialiseDatabaseAsync();
    app.MapScalarApiReference(options =>
    {
        options.Title = "AuthService API";
        options.ShowSidebar = true;
        options
            .WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "your-bearer-token";
            });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();