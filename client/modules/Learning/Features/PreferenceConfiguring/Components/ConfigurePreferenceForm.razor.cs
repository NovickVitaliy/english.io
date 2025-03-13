using System.Text.Json;
using Blazored.LocalStorage;
using Fluxor;
using Learning.Features.PreferenceConfiguring.Models;
using Learning.Features.PreferenceConfiguring.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using MudBlazor;
using Refit;
using Shared;
using Shared.Models;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Features.PreferenceConfiguring.Components;

public partial class ConfigurePreferenceForm : ComponentBase
{
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IUserPreferencesService UserPreferencesService { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    private readonly CreateUserPreferencesRequest _request = new CreateUserPreferencesRequest();
    private MudForm? _form = null!;

    private void HandleDailySessionsNumberChange(int sessionsCount)
    {
        if (sessionsCount > _request.DailySessionsReminderTimes.Count)
        {
            _request.DailySessionsReminderTimes.AddRange(Enumerable.Range(0, sessionsCount - _request.DailySessionsReminderTimes.Count).Select(_ => TimeSpan.Zero));
        }
        else
        {
            if (sessionsCount < _request.DailySessionsReminderTimes.Count)
            {
                _request.DailySessionsReminderTimes.RemoveRange(sessionsCount, _request.DailySessionsReminderTimes.Count - sessionsCount);
            }
        }
    }
    private async Task Submit()
    {
        await _form!.Validate();

        if (_form.IsValid)
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var token = UserState.Value.Token;
                _request.UserEmail = authState.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var jwtToken = await UserPreferencesService.CreateUserPreferencesAsync(_request, token);
                var json = await LocalStorageService.GetItemAsStringAsync(ClientConstants.UserDataKey);
                json = System.Text.RegularExpressions.Regex.Unescape(json!);
                var authData = JsonSerializer.Deserialize<UserData>(json!, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
                authData = authData! with
                {
                    AuthToken = jwtToken
                };
                await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, JsonSerializer.Serialize(authData));
                Dispatcher.Dispatch(new SetUserStateAction(authData.AuthToken, authData.Role, authData.Email, authData.Username, authData.IsEmailVerified));

                Snackbar.Add(Localizer["User_Preferences_Configured"], Severity.Success);
                await Task.Delay(2000);
                var query = $"?token={Uri.EscapeDataString(token)}";
                NavigationManager.NavigateTo("Learning/ConfigurePreference" + query, forceLoad: true);
            }
            catch (ApiException e)
            {
                Snackbar.Add(e.Content ?? "Error occured", Severity.Error);
            }
        }
    }
}
