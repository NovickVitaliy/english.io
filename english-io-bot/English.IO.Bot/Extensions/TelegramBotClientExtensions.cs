using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace English.IO.Bot.Extensions;

public static class TelegramBotClientExtensions
{
    private static readonly SemaphoreSlim s_semaphoreSlim = new SemaphoreSlim(1, 1);

    public static async Task<Message> SendMessageSynchronized(
        this ITelegramBotClient botClient,
        ChatId chatId,
        string text,
        ParseMode parseMode = default,
        ReplyParameters? replyParameters = default,
        IReplyMarkup? replyMarkup = default,
        LinkPreviewOptions? linkPreviewOptions = default,
        int? messageThreadId = default,
        IEnumerable<MessageEntity>? entities = default,
        bool disableNotification = default,
        bool protectContent = default,
        string? messageEffectId = default,
        string? businessConnectionId = default,
        bool allowPaidBroadcast = default,
        CancellationToken cancellationToken = default
    )
    {
        await s_semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            return await botClient.SendRequest(new SendMessageRequest
            {
                ChatId = chatId,
                Text = text,
                ParseMode = parseMode,
                ReplyParameters = replyParameters,
                ReplyMarkup = replyMarkup,
                LinkPreviewOptions = linkPreviewOptions,
                MessageThreadId = messageThreadId,
                Entities = entities,
                DisableNotification = disableNotification,
                ProtectContent = protectContent,
                MessageEffectId = messageEffectId,
                BusinessConnectionId = businessConnectionId,
                AllowPaidBroadcast = allowPaidBroadcast,
            }, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            s_semaphoreSlim.Release();
        }
    }
}
