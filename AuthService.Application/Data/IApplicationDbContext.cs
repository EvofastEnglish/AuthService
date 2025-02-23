using AuthService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Data;

public interface IApplicationDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}