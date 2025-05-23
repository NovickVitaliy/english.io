using System.Text.Json;
using Authentication.Features.Register.Models;
using Authentication.Shared.Services;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;
using Shared;
using Shared.Extensions;
using Shared.Store.User;
using Shared.Store.User.Actions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Authentication.Features.Register.Components;

public partial class RegisterForm : ComponentBase
{
    private bool _overlayVisible = false;
    private readonly RegisterRequest _registerRequest = new RegisterRequest();
    private bool _isValid = true;
    private MudForm _form = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; init; } = null!;

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            _overlayVisible = true;
            try
            {
                var response = await AuthenticationService.RegisterAsync(_registerRequest);
                await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, response);
                Dispatcher.Dispatch(new SetUserStateAction(response.AuthToken, response.Role, response.Email, response.Username, response.IsEmailVerified));

                var query = $"?token={Uri.EscapeDataString(response.AuthToken)}&" +
                            $"redirectUri=/preference-configuration";

                NavigationManager.NavigateTo("Authentication/SignToApp" + query, forceLoad: true);
                Snackbar.Add("Successful register. Welcome!", Severity.Success);
            }
            catch (ApiException e)
            {
                ProblemDetails problemDetails = e.ToProblemDetails();
                Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
            }
            finally
            {
                _overlayVisible = true;
            }
        }
    }
}
