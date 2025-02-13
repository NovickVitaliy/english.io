using Microsoft.EntityFrameworkCore;
using Notifications.API.Models;

namespace Notifications.API.Database;

public class NotificationsDbContext : DbContext
{
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    { }
}
