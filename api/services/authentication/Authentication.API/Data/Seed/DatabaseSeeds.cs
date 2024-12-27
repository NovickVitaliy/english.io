using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Data.Seed;

public static class DatabaseSeeds
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        await using var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.Add(new Role
            {
                Id = Guid.NewGuid(),
                Name = AuthConstants.Roles.User,
                NormalizedName = AuthConstants.Roles.User.ToUpperInvariant()
            });
            context.Roles.Add(new Role
            {
                Id = Guid.NewGuid(),
                Name = AuthConstants.Roles.Admin,
                NormalizedName = AuthConstants.Roles.Admin.ToUpperInvariant()
            });

            await context.SaveChangesAsync();
        }
    }
}