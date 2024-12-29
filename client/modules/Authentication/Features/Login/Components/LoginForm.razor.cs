using System.Text.Json;
using Authentication.Features.Login.Models;
using Authentication.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;

namespace Authentication.Features.Login.Components;

public partial class LoginForm : ComponentBase
{
    private readonly LoginRequest _loginRequest = new LoginRequest();
    private MudForm _form = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    
    private async Task Submit()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            try
            {
                var response = await AuthenticationService.LoginAsync(_loginRequest);
                Snackbar.Add("Successful login. Welcome!", Severity.Success);
                Snackbar.Add(JsonSerializer.Serialize(response), Severity.Info);
            }
            catch (ApiException e)
            {
                //TODO: only for development, change in future
                Snackbar.Add(e.Content ?? "Error occured during logging in", Severity.Error);
            }
        }
    }
}