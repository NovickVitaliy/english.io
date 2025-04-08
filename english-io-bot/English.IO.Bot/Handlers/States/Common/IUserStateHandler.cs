using English.IO.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace English.IO.Bot.Handlers.States.Common;

public interface IUserStateHandler
{
    bool CanHandle(UserState state);

    Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}