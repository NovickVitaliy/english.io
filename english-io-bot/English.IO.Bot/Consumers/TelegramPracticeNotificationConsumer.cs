using English.IO.Bot.Database;
using English.IO.Bot.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.MessageBus.Events.PracticeNotifications;
using Telegram.Bot;

namespace English.IO.Bot.Consumers;

public class TelegramPracticeNotificationConsumer : IConsumer<TelegramPracticeNotification>
{
    private readonly ITelegramBotClient _botClient;
    private readonly EnglishIOBotDbContext _dbContext;

    public TelegramPracticeNotificationConsumer(ITelegramBotClient botClient, EnglishIOBotDbContext dbContext)
    {
        _botClient = botClient;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TelegramPracticeNotification> context)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserEmail == context.Message.Email);
        if (user is null)
        {
            return;
        }

        var telegramChatId = user.TelegramChatId;
        await _botClient.SendMessageSynchronized(telegramChatId, "Hi! Its time to practice your english.");
    }
}
