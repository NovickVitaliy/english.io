using Fluxor;
using Learning.Features.Settings.Models;
using Learning.Features.Settings.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Settings.Components.SecurityComponent;

public partial class ChangePasswordComponent : ComponentBase
{
    private MudForm? _form = null;
    private readonly ChangePasswordRequest _request = new ChangePasswordRequest();
    private bool _overlayVisible = false;
    [Inject] private IStringLocalizer<ChangePasswordComponent> Localizer { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IAuthenticationSettingsService AuthenticationSettingsService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;

    private async Task ChangePasswordAsync()
    {
        await _form!.Validate();
        if(!_form.IsValid) return;
        _overlayVisible = true;
        try
        {
            await AuthenticationSettingsService.ChangePasswordAsync(_request, UserState.Value.Token);
            Snackbar.Add(Localizer["Password_Changed_Successfully"], Severity.Success);
            await _form.ResetAsync();
        }
        catch (ApiException e)
        {
            ProblemDetails problemDetails = e.ToProblemDetails();
            Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
        }
        finally
        {
            _overlayVisible = false;
        }
    }
}

