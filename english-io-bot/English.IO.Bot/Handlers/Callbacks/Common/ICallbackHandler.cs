using Telegram.Bot;
using Telegram.Bot.Types;

namespace English.IO.Bot.Handlers.Callbacks.Common;

public interface ICallbackHandler
{
    bool CanHandle(string callbackGroup);

    Task Handle(ITelegramBotClient client, Update update, CancellationToken cancellationToken);
}