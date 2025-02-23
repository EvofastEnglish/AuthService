using AuthService.Domain.Models;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Extensions;

public class EmailOrUsernamePasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public EmailOrUsernamePasswordValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        ApplicationUser? user = null;

        if (context.UserName.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(context.UserName);
        }
        else
        {
            user = await _userManager.FindByNameAsync(context.UserName);
        }

        if (user == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            return;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, false);
        if (!result.Succeeded)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            return;
        }

        context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
    }
}
