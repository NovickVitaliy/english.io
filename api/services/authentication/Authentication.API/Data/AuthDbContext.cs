using System.Reflection;
using Authentication.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Data;

public class AuthDbContext : IdentityDbContext<User, Role, Guid>
{
    public const string DefaultConnectionStringPosition = "Default";

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}