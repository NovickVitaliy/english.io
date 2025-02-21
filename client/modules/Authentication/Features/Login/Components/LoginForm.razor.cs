using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Authentication.Features.Login.Models;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using MudBlazor;
using Refit;
using Shared;
using Shared.Extensions;
using Shared.Store;
using Shared.Store.User;
using IAuthenticationService = Authentication.Shared.Services.IAuthenticationService;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Authentication.Features.Login.Components;

public partial class LoginForm : ComponentBase
{
    [SupplyParameterFromQuery] private string? ReturnUrl { get; init; } = null!;
    private readonly LoginRequest _loginRequest = new LoginRequest();
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
            try
            {
                var response = await AuthenticationService.LoginAsync(_loginRequest);
                await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, response);
                Dispatcher.Dispatch(new SetUserStateAction(response.AuthToken, response.Role, response.Email, response.Username));

                var query = $"?token={Uri.EscapeDataString(response.AuthToken)}" +
                            (ReturnUrl is not null ? $"&redirectUri={Uri.EscapeDataString(ReturnUrl)}" : "");
                NavigationManager.NavigateTo("Authentication/SignToApp" + query, forceLoad: true);
                Snackbar.Add("Successful login. Welcome!", Severity.Success);
            }
            catch (ApiException e)
            {
                ProblemDetails problemDetails = e.ToProblemDetails();
                Snackbar.Add((problemDetails!.Detail ?? problemDetails.Title)!, Severity.Error);
            }
        }
    }
}
