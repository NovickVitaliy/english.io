using English.IO.Bot.Database;
using English.IO.Bot.Extensions;
using English.IO.Bot.Handlers.States.Common;
using English.IO.Bot.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBus.Events;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace English.IO.Bot.Handlers.States;

public class SubmittingCodeStateHandler : IUserStateHandler
{
    private const UserState HandledState = UserState.SubmittingCode;
    private readonly EnglishIOBotDbContext _dbContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubmittingCodeStateHandler(EnglishIOBotDbContext dbContext, IServiceScopeFactory serviceScopeFactory)
    {
        _dbContext = dbContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public bool CanHandle(UserState state) => state == HandledState;

    public async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message!.Chat.Id;
        var code = update.Message.Text?.Trim();
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        if (code is null)
        {
            await botClient.SendMessageSynchronized(chatId, "Incorrect code", cancellationToken: cancellationToken);
            return;
        }

        await publishEndpoint.Publish(new UserTypedInTelegramNotificationsConfigurationCode(code, chatId), cancellationToken);
        await botClient.SendMessageSynchronized(chatId, "Code was sent. Waiting for the response...", cancellationToken: cancellationToken);
    }
}
