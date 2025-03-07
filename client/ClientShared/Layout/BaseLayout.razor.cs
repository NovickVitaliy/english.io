using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Store.Theme;
using Shared.Store.User;

namespace Shared.Layout;

public abstract partial class BaseLayout : LayoutComponentBase, IDisposable
{
    private bool _disposed;
    protected readonly MudTheme Theme = new();
    protected bool IsDarkMode;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IState<ThemeState> ThemeState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ThemeState.StateChanged += OnThemeStateChanged;
            await LoadUserFromStorage();
            await LoadThemeFromStorage();
            StateHasChanged();
        }
    }

    private async void OnThemeStateChanged(object? sender, EventArgs eventArgs)
    {
        IsDarkMode = ThemeState.Value.IsDarkTheme;
        await LocalStorageService.SetItemAsync(ClientConstants.ThemeKey, JsonSerializer.Serialize(ThemeState.Value));
    }

    private async Task LoadThemeFromStorage()
    {
        var themeDataJson = await LocalStorageService.GetItemAsync<string>(ClientConstants.ThemeKey);
        if (themeDataJson is not null)
        {
            var action = JsonSerializer.Deserialize<SetThemeStateAction>(themeDataJson);
            Dispatcher.Dispatch(action);
        }
        else
        {
            await LocalStorageService.RemoveItemAsync(ClientConstants.ThemeKey);
        }
    }

    private async Task LoadUserFromStorage()
    {
        var userDataJson = await LocalStorageService.GetItemAsync<string?>(ClientConstants.UserDataKey);
        if (userDataJson is not null)
        {
            var action = JsonSerializer.Deserialize<SetUserStateAction?>(userDataJson);
            if (action is not null && IsJwtTokenValid(action.AuthToken))
            {
                Dispatcher.Dispatch(action);
            }
            else
            {
                await LocalStorageService.RemoveItemAsync(ClientConstants.UserDataKey);
                NavigationManager.NavigateTo("/login");
            }
        }
        NavigationManager.NavigateTo("/login");
    }

    private static bool IsJwtTokenValid(string jwtToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);

        return jwt.ValidTo >= DateTime.UtcNow;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                UserState.StateChanged -= OnThemeStateChanged;
            }

            _disposed = true;
        }
    }

    ~BaseLayout()
    {
        Dispose(false);
    }
}
