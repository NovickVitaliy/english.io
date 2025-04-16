using System.ComponentModel.Design;
using English.IO.Bot.Extensions;
using Microsoft.Extensions.Hosting;

namespace English.IO.Bot.BackgroundTasks;

public class ResourceManagerHostedService : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public ResourceManagerHostedService(IHostApplicationLifetime hostApplicationLifetime)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _hostApplicationLifetime.ApplicationStopping.Register(OnApplicationStopping);
        return Task.CompletedTask;
    }

    private void OnApplicationStopping()
    {
        TelegramBotClientExtensions.Dispose();
    }
}
