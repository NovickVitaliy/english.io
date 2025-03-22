using Authentication.Features.ResetPassword.Models;
using Authentication.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Options;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Authentication.Features.ResetPassword.Components;

public partial class ResetPasswordForm : ComponentBase
{
    [SupplyParameterFromQuery]
    private string Token { get; init; } = null!;

    [SupplyParameterFromQuery]
    private string Email { get; init; } = null!;
    private MudForm? _form = null!;
    private ResetPasswordRequest _request = new();
    private bool _overlayVisible = false;

    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IOptions<ClientOptions> ClientOptions { get; init; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;

    protected override void OnParametersSet()
    {
        if (string.IsNullOrWhiteSpace(Token) || string.IsNullOrWhiteSpace(Email))
        {
            NavigationManager.NavigateTo(ClientOptions.Value.GetClientBaseUrl().ToString());
        }
        _request = new ResetPasswordRequest
        {
            ResetToken = Token,
            Email = Email
        };
    }

    private async Task Submit()
    {
        await _form!.Validate();
        if (!_form.IsValid) return;

        _overlayVisible = true;
        try
        {
            await AuthenticationService.ResetPasswordAsync(_request);
            Snackbar.Add(Localizer["Password_Reset_Successfully"], Severity.Success);
            await Task.Delay(TimeSpan.FromSeconds(2));
            NavigationManager.NavigateTo("login");
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

