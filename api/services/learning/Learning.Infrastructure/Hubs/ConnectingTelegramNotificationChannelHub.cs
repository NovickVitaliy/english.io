using Learning.Infrastructure.Helpers;
using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Learning.Infrastructure.Hubs;

public class ConnectingTelegramNotificationChannelHub : Hub
{
    private const int TelegramConnectCodeLenght = 6;
    public override async Task OnConnectedAsync()
    {
        var code = RandomStringHelper.GenerateRandomString(TelegramConnectCodeLenght);
        await Clients.Caller.SendAsync(ConnectingTelegramNotificationChannelHubMessages.UserConnected, code);
    }
}

