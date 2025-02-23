using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace AuthService.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString("Database")!);
        return services;
    }
    
    public static WebApplication UseApiServices(this WebApplication app)
    {

        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();
        return app;
    }
}