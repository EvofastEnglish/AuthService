using AuthService.Application.Data;
using AuthService.Domain.Models;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Interceptors;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer(options =>
            {
                options.IssuerUri = "https://evofast-identityserver.solocode.click";
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly("AuthService.Infrastructure"));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly("AuthService.Infrastructure"));
            })    
            .AddProfileService<CustomProfileService>();
        services.AddScoped<IResourceOwnerPasswordValidator, EmailOrUsernamePasswordValidator>();

        
        services.AddAuthorization();

        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        return services;
    }
}