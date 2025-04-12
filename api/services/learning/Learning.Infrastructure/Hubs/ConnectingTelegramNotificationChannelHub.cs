using Learning.Infrastructure.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;

namespace Learning.Infrastructure.Hubs;

public class ConnectingTelegramNotificationChannelHub : Hub
{
    private const int TelegramConnectCodeLenght = 6;
    private readonly IDistributedCache _distributedCache;

    public ConnectingTelegramNotificationChannelHub(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public override async Task OnConnectedAsync()
    {
        var cacheKey = $"{Context.ConnectionId}-code";
        var code = await _distributedCache.GetStringAsync(cacheKey);
        if (string.IsNullOrWhiteSpace(code))
        {
            code = RandomStringHelper.GenerateRandomString(TelegramConnectCodeLenght);
            await _distributedCache.SetStringAsync(cacheKey, code);
            await _distributedCache.SetStringAsync(code, Context.ConnectionId);
        }
        await Clients.Caller.SendAsync(ConnectingTelegramNotificationChannelHubMessages.UserConnected, code);
    }
}

