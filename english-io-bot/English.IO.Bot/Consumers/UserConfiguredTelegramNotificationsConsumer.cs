using English.IO.Bot.Database;
using English.IO.Bot.Extensions;
using English.IO.Bot.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.MessageBus.Events;
using Telegram.Bot;

namespace English.IO.Bot.Consumers;

public class UserConfiguredTelegramNotificationsConsumer : IConsumer<UserConfiguredTelegramNotifications>
{
    private readonly EnglishIOBotDbContext _dbContext;
    private readonly ITelegramBotClient _telegramBotClient;

    public UserConfiguredTelegramNotificationsConsumer(EnglishIOBotDbContext dbContext, ITelegramBotClient telegramBotClient)
    {
        _dbContext = dbContext;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Consume(ConsumeContext<UserConfiguredTelegramNotifications> context)
    {
        var chatId = context.Message.ChatId;
        var success = context.Message.Success;
        if (success)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramChatId == chatId);
            if (user is null) return;
            if (user.HasSubmittedCode)
            {
                await _telegramBotClient.SendMessageSynchronized(chatId, "You have already configured notifications via telegram.");
                return;
            }
            user.HasSubmittedCode = true;
            user.UserEmail = context.Message.UserEmail;
            await _dbContext.SaveChangesAsync();
            await _telegramBotClient.SendMessageSynchronized(chatId, "You have successfuly configured notifications via telegram.");
        }
        else
        {
            await _telegramBotClient.SendMessageSynchronized(chatId, "There was an error configuring notifications via telegram.");
        }
    }
}
