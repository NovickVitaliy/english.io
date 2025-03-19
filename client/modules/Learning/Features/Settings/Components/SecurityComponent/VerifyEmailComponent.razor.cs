using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Settings.Models.Security;
using Learning.Features.Settings.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Options;
using Shared.Store.User;

namespace Learning.Features.Settings.Components.SecurityComponent;

public partial class VerifyEmailComponent : FluxorComponent
{
    [Inject]
    private IStringLocalizer<VerifyEmailComponent> Localizer { get; init; } = null!;

    [Inject]
    private IState<UserState> UserState { get; init; } = null!;

    [Inject]
    private IOptions<ClientOptions> ClientOptions { get; init; } = null!;

    [Inject]
    private IAuthenticationSettingsService AuthenticationSettingsService { get; init; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; init; } = null!;

    private async Task VerifyEmailAsync()
    {
        try
        {
            var request = new SendVerifyingEmailMessageRequest(new Uri(ClientOptions.Value.GetClientBaseUrl(), "verify-email"));
            await AuthenticationSettingsService.SendVerifyingEmailMessageAsync(request, UserState.Value.Token);
            Snackbar.Add(Localizer["Verification_Email_Was_Sent"], Severity.Success);
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
        }
    }
}

