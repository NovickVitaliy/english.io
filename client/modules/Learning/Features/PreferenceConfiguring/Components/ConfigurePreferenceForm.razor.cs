using System.Security.Claims;
using Learning.Features.PreferenceConfiguring.Models;
using Learning.Features.PreferenceConfiguring.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Refit;

namespace Learning.Features.PreferenceConfiguring.Components;

public partial class ConfigurePreferenceForm : ComponentBase
{
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IUserPreferencesService UserPreferencesService { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    private CreateUserPreferencesRequest _request = new CreateUserPreferencesRequest();
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
                _request.UserEmail = authState.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;
                await UserPreferencesService.CreateUserPreferencesAsync(_request);

                Snackbar.Add(Localizer["User_Preferences_Configured"], Severity.Success);
                await Task.Delay(2000);
                NavigationManager.NavigateTo("/learning/home");
            }
            catch (ApiException e)
            {
                Snackbar.Add(e.Content ?? "Error occured", Severity.Error);
            }
        }
    }
}