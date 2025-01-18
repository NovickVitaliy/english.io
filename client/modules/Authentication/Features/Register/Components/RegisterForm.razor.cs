using System.Text.Json;
using Authentication.Features.Register.Models;
using Authentication.Shared.Services;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;
using Shared;
using Shared.Store;

namespace Authentication.Features.Register.Components;

public partial class RegisterForm : ComponentBase
{
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
            try
            {
                var response = await AuthenticationService.RegisterAsync(_registerRequest);
                await LocalStorageService.SetItemAsync(ClientConstants.UserDataKey, JsonSerializer.Serialize(response));
                Dispatcher.Dispatch(new SetUserStateAction(response.AuthToken, response.Role, response.Email, response.Username));
                
                var query = $"?token={Uri.EscapeDataString(response.AuthToken)}&" +
                            $"redirectUri=/preference-configuration";

                NavigationManager.NavigateTo("Authentication/SignToApp" + query, forceLoad: true);
                Snackbar.Add("Successful register. Welcome!", Severity.Success);
            }
            catch (ApiException e)
            {
                Snackbar.Add(e.Content ?? "Error occured during registration", Severity.Error);
            }
        }
    }
}