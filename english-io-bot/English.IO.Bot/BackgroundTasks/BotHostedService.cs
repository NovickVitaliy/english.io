using English.IO.Bot.Handlers;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace English.IO.Bot.BackgroundTasks;

public class BotHostedService : IHostedService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly UpdateHandler _updateHandler;

    public BotHostedService(UpdateHandler updateHandler, ITelegramBotClient telegramBotClient)
    {
        _updateHandler = updateHandler;
        _telegramBotClient = telegramBotClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var commands = GetTelegramBotCommands();

        await _telegramBotClient.SetMyCommands(commands, cancellationToken: cancellationToken);
        _telegramBotClient.StartReceiving(_updateHandler.HandleUpdateAsync, _updateHandler.HandleErrorAsync, cancellationToken: cancellationToken);
    }
    private static BotCommand[] GetTelegramBotCommands()
        => [
            new BotCommand {Command = Constants.Commands.Start, Description = "Почати роботу з ботом"},
        ];

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
