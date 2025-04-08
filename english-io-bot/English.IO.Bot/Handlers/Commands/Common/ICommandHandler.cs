using Telegram.Bot;
using Telegram.Bot.Types;

namespace English.IO.Bot.Handlers.Commands.Common;

public interface ICommandHandler
{
    bool CanHandle(string command);
    Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}