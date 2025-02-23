using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}