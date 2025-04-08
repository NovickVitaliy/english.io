using Learning.Features.PreferenceConfiguring.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shared;

namespace Learning.Features.PreferenceConfiguring.Pages;

public partial class ConnectingTelegramNotifications : ComponentBase
{
    [Inject] private IStringLocalizer<ConnectingTelegramNotifications> Localizer { get; init; } = null!;
    [Inject] private IOptions<PreferencesConfiguringHubOptions> PreferencesConfiguringHubOptions { get; init; } = null!;
    private HubConnection? _hubConnection;
    private string? _code;

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(PreferencesConfiguringHubOptions.Value.HubUrl)
            .Build();

        _hubConnection.On<string>(ConnectingTelegramNotificationChannelHubMessages.UserConnected, async (code) =>
        {
            if (_code is null)
            {
                _code = code;
                await InvokeAsync(StateHasChanged);
            }
        });

        await _hubConnection.StartAsync();
    }
}

