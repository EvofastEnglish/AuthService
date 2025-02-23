using AuthService.Domain.Models;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Extensions;

public class InitialData
{
    public static IEnumerable<ApplicationUser> Users =>
    [
        new ApplicationUser
        {
            FirstName = "Lieu",
            LastName = "Dang",
            UserName = "lieudvq",
            Email = "lieudvq0302@gmail.com",
        },
    ];
    
    public static IEnumerable<ApplicationRole> Roles =>
    [
        new ApplicationRole
        {
            Name = "Admin", NormalizedName = "ADMIN", Id = Guid.Parse("ef343efc-8904-4d12-b340-36c41d338306")
        },
        new ApplicationRole
        {
            Name = "User", NormalizedName = "USER", Id = Guid.Parse("003f7676-1d91-4143-9bfd-7a6c17c156fe")
        },
    ];
    
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Address(),
        new IdentityResources.Email(),
        new IdentityResource
            {
                Name = "roles",
                Description = "Your role(s)",
                UserClaims = new List<string> { "role" }
            },
    ];
    
    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("EvoFastAPI.read"), 
            new ApiScope("EvoFastAPI.write")
        ];
    
    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("EvoFastAPI")
            {
                Scopes = new List<string> { "EvoFastAPI.read", "EvoFastAPI.write" },
                ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
                UserClaims = new List<string> { "roles" }
            }
    ];
    
    public static IEnumerable<Client> Clients =>
    [
        new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
                AllowedScopes =
                {
                    "EvoFastAPI.read", "EvoFastAPI.write",
                    "roles"
                },
                AllowOfflineAccess = true,
            }
    ];
}