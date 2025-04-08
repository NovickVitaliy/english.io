using English.IO.Bot.Handlers.Callbacks.Common;
using English.IO.Bot.Handlers.Commands.Common;
using English.IO.Bot.Handlers.States.Common;
using English.IO.Bot.Managers.UserStates;
using English.IO.Bot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace English.IO.Bot.Handlers;

public class UpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IEnumerable<ICommandHandler> _commandHandlers;
    private readonly IEnumerable<IUserStateHandler> _userStateHandlers;
    private readonly IEnumerable<ICallbackHandler> _callbackHandlers;
    private readonly IUserStateManager _userStateManager;
    
    public UpdateHandler(ILogger<UpdateHandler> logger, IEnumerable<ICommandHandler> commandHandlers,
        IEnumerable<IUserStateHandler> userStateHandlers, IUserStateManager userStateManager, IEnumerable<ICallbackHandler> callbackHandlers)
    {
        _logger = logger;
        _commandHandlers = commandHandlers;
        _userStateHandlers = userStateHandlers;
        _userStateManager = userStateManager;
        _callbackHandlers = callbackHandlers;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery is not null)
        {
            await HandleCallbackQuery(botClient, update, cancellationToken);
            return;
        }
        var text = update.Message?.Text;
        var userTelegramId = update.Message!.From!.Id;
        var handledState = await HandleStateIfExists(userTelegramId, botClient, update, cancellationToken);
        if (!handledState)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            await HandleCommandIfExistsAsync(text, botClient, update, cancellationToken);
        }
    }
    private async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = _callbackHandlers.SingleOrDefault(x => x.CanHandle(update.CallbackQuery!.Data!.Split('_')[0]));
        if (handler is not null)
        {
            await handler.Handle(botClient, update, cancellationToken);
        }
    }

    private async Task<bool> HandleCommandIfExistsAsync(string text, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message!.Chat.Id;
        var handler = _commandHandlers.SingleOrDefault(x => x.CanHandle(text));
        if (handler is not null)
        {
            await handler.Handle(botClient, update, cancellationToken);
            return true;
        }

        await botClient.SendMessage(chatId, "Упс, схоже ви ввели невідому команду... \nВведіть дійсну команду або напишіть /help, якщо ви забули 🙂",
            cancellationToken: cancellationToken);
        return false;
    }

    private async Task<bool> HandleStateIfExists(long userTelegramId, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var state = await _userStateManager.GetStateForUser(userTelegramId);
        if (state is UserState.None)
            return false;
        var stateHandler = _userStateHandlers.SingleOrDefault(x => x.CanHandle(state));
        if (stateHandler is null)
            return false;
        await stateHandler.Handle(botClient, update, cancellationToken);
        return true;
    }

    public Task HandleErrorAsync(ITelegramBotClient telegramBotClient, Exception exception, HandleErrorSource handleErrorSource, CancellationToken cancellationToken)
    {
        _logger.LogError("Error occurred: {Message}", exception.Message);
        return Task.CompletedTask;
    }
}