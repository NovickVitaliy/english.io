using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Settings.Models.Security;
using Learning.Features.Settings.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared;
using Shared.Extensions;
using Shared.Models;
using Shared.Store.User;
using Shared.Store.User.Actions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Settings.Components.SecurityComponent;

public partial class EmailVerificationComponent : FluxorComponent
{
    [Inject]
    private IStringLocalizer<EmailVerificationComponent> Localizer { get; init; } = null!;

    [Inject]
    private IAuthenticationSettingsService AuthenticationSettingsService { get; init; } = null!;

    [Inject]
    private IState<UserState> UserState { get; init; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; init; } = null!;

    [Inject]
    private IDispatcher Dispatcher { get; init; } = null!;

    [Inject]
    private ILocalStorageService LocalStorageService { get; init; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; init; } = null!;

    [SupplyParameterFromQuery]
    private string Token { get; init; } = null!;

    private async Task VerifyEmail()
    {
        try
        {
            var newAccessToken = await AuthenticationSettingsService.VerifyEmailAsync(new VerifyEmailRequest(Token), UserState.Value.Token);
            Dispatcher.Dispatch(new VerifyUserEmailAction());
            Dispatcher.Dispatch(new SetNewAccessTokenAction(newAccessToken));
            var userData = UserState.Value;
            await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, new UserData(userData.Username, userData.Email, userData.Role, newAccessToken, true));
            Snackbar.Add(Localizer["Email_Verified_Successfully"], Severity.Success);
            await Task.Delay(TimeSpan.FromSeconds(2));
            NavigationManager.NavigateTo("/learning/home");
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(problemDetails.Title ?? problemDetails.Detail ?? Localizer["Error_Occured"], Severity.Error);
        }
    }
}

