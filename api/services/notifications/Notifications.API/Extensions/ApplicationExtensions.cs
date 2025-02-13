using Microsoft.EntityFrameworkCore;
using Notifications.API.Database;

namespace Notifications.API.Extensions;

public static class ApplicationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication host)
    {
        await using var serviceScope = host.Services.CreateAsyncScope();
        var notificationsDbContext = serviceScope.ServiceProvider.GetRequiredService<NotificationsDbContext>();

        await notificationsDbContext.Database.MigrateAsync();
    }
}
