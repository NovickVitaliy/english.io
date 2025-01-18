using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Store;

namespace Shared.Layout;

public abstract partial class BaseLayout : LayoutComponentBase
{
    protected readonly MudTheme Theme = new();
    protected bool IsDarkMode;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadUserFromStorage();
            StateHasChanged();
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
            }
        }
    }

    private static bool IsJwtTokenValid(string jwtToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);

        return jwt.ValidTo >= DateTime.UtcNow;
    }
}