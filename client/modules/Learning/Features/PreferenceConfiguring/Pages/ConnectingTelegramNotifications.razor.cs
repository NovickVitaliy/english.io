using Learning.Features.PreferenceConfiguring.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using Shared;

namespace Learning.Features.PreferenceConfiguring.Pages;

public partial class ConnectingTelegramNotifications : ComponentBase, IDisposable
{
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IStringLocalizer<ConnectingTelegramNotifications> Localizer { get; init; } = null!;
    [Inject] private IOptions<PreferencesConfiguringHubOptions> PreferencesConfiguringHubOptions { get; init; } = null!;
    private HubConnection? _hubConnection;
    private string? _code;
    private bool _disposed;

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

        _hubConnection.On(ConnectingTelegramNotificationChannelHubMessages.NotificationsConfigured, async () =>
        {
            Snackbar.Add(Localizer["Telegram_Notifications_Configured"], Severity.Success);
            await Task.Delay(TimeSpan.FromSeconds(2));
            NavigationManager.NavigateTo("/learning/home");
        });

        await _hubConnection.StartAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_disposed)
            {
                return;
            }
            _hubConnection?.DisposeAsync().AsTask();
            _disposed = true;
        }
    }
}

