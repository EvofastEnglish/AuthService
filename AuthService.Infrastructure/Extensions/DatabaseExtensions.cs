using AuthService.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
        
        await SeedConfigAsync(configurationDbContext);
        await SeedRoleAsync(roleManager);
        await SeedUserAsync(userManager);
    }
    
    private static async Task SeedConfigAsync(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            foreach (var client in InitialData.Clients.ToList())
            {
                context.Clients.Add(client.ToEntity());
            }
            await context.SaveChangesAsync();
        }
        
        if (!context.IdentityResources.Any())
        {
            foreach (var resource in InitialData.IdentityResources.ToList())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }
        
        if (!context.ApiScopes.Any())
        {
            foreach (var resource in InitialData.ApiScopes.ToList())
            {
                context.ApiScopes.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }
        
        if (!context.ApiResources.Any())
        {
            foreach (var resource in InitialData.ApiResources.ToList())
            {
                context.ApiResources.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }
    }
    
    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            foreach (var user in InitialData.Users)
            {
                await userManager.CreateAsync(user, "Password123@");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }

    private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            foreach (var role in InitialData.Roles)
            {
                await roleManager.CreateAsync(role);
            }
        }   
    }
}
