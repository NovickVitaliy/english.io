using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace English.IO.Bot.Database;

public class EnglishIOBotDbContext : DbContext
{
    public const string ConnectionStringKey = "Default";
    public DbSet<Models.User> Users => Set<Models.User>();

    public EnglishIOBotDbContext(DbContextOptions<EnglishIOBotDbContext> options)
        : base(options)
    { }
}
