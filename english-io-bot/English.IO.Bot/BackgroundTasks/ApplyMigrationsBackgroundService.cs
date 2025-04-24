using English.IO.Bot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace English.IO.Bot.BackgroundTasks;

public class ApplyMigrationsBackgroundService : IHostedService
{
    private readonly EnglishIOBotDbContext _dbContext;

    public ApplyMigrationsBackgroundService(EnglishIOBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
