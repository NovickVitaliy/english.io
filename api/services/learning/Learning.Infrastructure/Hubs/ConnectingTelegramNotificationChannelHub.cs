using Learning.Infrastructure.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Services.Contracts;

namespace Learning.Infrastructure.Hubs;

public class ConnectingTelegramNotificationChannelHub : Hub
{
    private const int TelegramConnectCodeLenght = 6;
    private readonly IDistributedCache _distributedCache;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public ConnectingTelegramNotificationChannelHub(
        IDistributedCache distributedCache,
        ICurrentUserAccessor currentUserAccessor)
    {
        _distributedCache = distributedCache;
        _currentUserAccessor = currentUserAccessor;
    }

    public override async Task OnConnectedAsync()
    {
        var email = _currentUserAccessor.GetEmail();
        var cacheKey = $"{Context.ConnectionId}-code";
        var code = await _distributedCache.GetStringAsync(cacheKey);
        if (string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(email))
        {
            code = RandomStringHelper.GenerateRandomString(TelegramConnectCodeLenght);
            await _distributedCache.SetStringAsync(cacheKey, code);
            await _distributedCache.SetStringAsync(code, Context.ConnectionId);
            await _distributedCache.SetStringAsync($"{code}-email", email!);
        }
        await Clients.Caller.SendAsync(ConnectingTelegramNotificationChannelHubMessages.UserConnected, code);
    }
}

