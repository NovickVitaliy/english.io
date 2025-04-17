using Learning.Infrastructure.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.MessageBus.Events;

namespace Learning.Infrastructure.Consumers;

public class UserTypedInTelegramNotificationsConfigurationCodeConsumer : IConsumer<UserTypedInTelegramNotificationsConfigurationCode>
{
    private readonly IHubContext<ConnectingTelegramNotificationChannelHub> _hubContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _distributedCache;

    public UserTypedInTelegramNotificationsConfigurationCodeConsumer(
        IHubContext<ConnectingTelegramNotificationChannelHub> hubContext,
        IPublishEndpoint publishEndpoint,
        IDistributedCache distributedCache)
    {
        _hubContext = hubContext;
        _publishEndpoint = publishEndpoint;
        _distributedCache = distributedCache;
    }

    public async Task Consume(ConsumeContext<UserTypedInTelegramNotificationsConfigurationCode> context)
    {
        var code = context.Message.Code;
        var chatId = context.Message.ChatId;
        var connectionId = await _distributedCache.GetStringAsync(code);
        var email = await _distributedCache.GetStringAsync($"{code}-email");
        if (!string.IsNullOrWhiteSpace(connectionId))
        {
            await _hubContext.Clients.Client(connectionId).SendAsync(ConnectingTelegramNotificationChannelHubMessages.NotificationsConfigured);
            await _publishEndpoint.Publish(new UserConfiguredTelegramNotifications(chatId, email!, true));
            await _distributedCache.RemoveAsync(code);
            await _distributedCache.RemoveAsync($"{code}-email");
            await _distributedCache.RemoveAsync($"{connectionId}-code");
        }
        else
        {
            await _publishEndpoint.Publish(new UserConfiguredTelegramNotifications(chatId, email!, false));
        }
    }
}
