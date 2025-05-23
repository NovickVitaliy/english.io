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
using Shared.Extensions;
using Shared.Models;
using Shared.Store;
using Shared.Store.User;
using Shared.Store.User.Actions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

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
    private bool _overlayVisible = false;

    private void HandleDailySessionsNumberChange(int sessionsCount)
    {
        _request.DailySessionsCount = sessionsCount;
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
            _overlayVisible = true;
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var token = UserState.Value.Token;
                _request.UserEmail = authState.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                _request.TimezoneId = TimeZoneInfo.Local.Id;
                var jwtToken = await UserPreferencesService.CreateUserPreferencesAsync(_request, token);
                var json = await LocalStorageService.GetItemAsStringAsync(ClientConstants.UserDataKey);
                json = System.Text.RegularExpressions.Regex.Unescape(json!);
                var authData = JsonSerializer.Deserialize<UserData>(json!, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                authData = authData! with
                {
                    AuthToken = jwtToken
                };
                await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, JsonSerializer.Serialize(authData));
                Dispatcher.Dispatch(new SetUserStateAction(authData.AuthToken, authData.Role, authData.Email, authData.Username, authData.IsEmailVerified));

                var isTelegram = _request.NotificationChannel == NotificationChannel.Telegram;

                await Task.Delay(2000);
                var query = $"?token={Uri.EscapeDataString(token)}";
                if (isTelegram)
                {
                    query += "&isTelegram=true";
                }
                else
                {
                    query += "&isTelegram=false";
                }
                NavigationManager.NavigateTo("Learning/ConfigurePreference" + query, forceLoad: true);
            }
            catch (ApiException e)
            {
                var problemDetails = e.ToProblemDetails();
                Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
            }
            finally
            {
                _overlayVisible = false;
            }
        }
    }
}
