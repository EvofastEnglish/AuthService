using AuthService.API;
using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(builder.Configuration["AllowedOrigins"]);
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
// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    await app.InitialiseDatabaseAsync();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Evofast IdentityServer 0.0.1";
        options.ShowSidebar = true;
        options
            .WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "your-bearer-token";
            });
    });
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();