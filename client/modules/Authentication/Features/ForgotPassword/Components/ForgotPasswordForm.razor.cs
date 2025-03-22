using Authentication.Features.ForgotPassword.Models;
using Authentication.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Options;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Authentication.Features.ForgotPassword.Components;

public partial class ForgotPasswordForm : ComponentBase
{
    private ForgotPasswordRequest _forgotPasswordRequest = null!;
    private MudForm _form = null!;
    private bool _overlayVisible = false;
    [Inject] private IOptions<ClientOptions> ClientOptions { get; init; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;

    protected override void OnParametersSet()
    {
        _forgotPasswordRequest = new ForgotPasswordRequest()
        {
            ResetPasswordUrl = new Uri(ClientOptions.Value.GetClientBaseUrl(), "reset-password")
        };
    }

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid) return;
        _overlayVisible = true;
        try
        {
            await AuthenticationService.ForgotPasswordAsync(_forgotPasswordRequest);
            Snackbar.Add(Localizer["Email_Sent_Successfully"], Severity.Success);
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

