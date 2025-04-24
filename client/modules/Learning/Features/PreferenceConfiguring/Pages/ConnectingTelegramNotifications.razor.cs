using Fluxor;
using Learning.Features.PreferenceConfiguring.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using Shared;
using Shared.Store.User;

namespace Learning.Features.PreferenceConfiguring.Pages;

public partial class ConnectingTelegramNotifications : ComponentBase, IDisposable
{
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IStringLocalizer<ConnectingTelegramNotifications> Localizer { get; init; } = null!;
    [Inject] private IOptions<PreferencesConfiguringHubOptions> PreferencesConfiguringHubOptions { get; init; } = null!;
    private HubConnection? _hubConnection;
    private string? _code;
    private bool _disposed;

    protected override Task OnInitializedAsync()
    {
        UserState.StateChanged += async (_, _) =>
        {
            if (!string.IsNullOrWhiteSpace(UserState.Value.Email))
            {
                await ConnectToTheHub();
            }
        };

        return Task.CompletedTask;
    }
    private async Task ConnectToTheHub()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(PreferencesConfiguringHubOptions.Value.HubUrl, options =>
                options.AccessTokenProvider = () => Task.FromResult(UserState.Value.Token)!)
            .Build();

        _hubConnection.On<string>(ConnectingTelegramNotificationChannelHubMessages.UserConnected, async (code) =>
        {
            if (_code is null)
            {
                Console.WriteLine(code);
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
