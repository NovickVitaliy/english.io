using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.Features.Login.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using Refit;
using IAuthenticationService = Authentication.Shared.Services.IAuthenticationService;

namespace Authentication.Features.Login.Components;

public partial class LoginForm : ComponentBase
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    private readonly LoginRequest _loginRequest = new LoginRequest();
    private MudForm _form = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    
    private async Task Submit()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            try
            {
                var response = await AuthenticationService.LoginAsync(_loginRequest);
                var uri = new Uri(NavigationManager.Uri)
                    .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

                var query = $"?token={Uri.EscapeDataString(response.AuthToken)}&" +
                            $"redirectUri={Uri.EscapeDataString(uri)}";

                NavigationManager.NavigateTo("Authentication/Login" + query, forceLoad: true);
                Snackbar.Add("Successful login. Welcome!", Severity.Success);
            }
            catch (ApiException e)
            {
                //TODO: only for development, change in future
                Snackbar.Add(e.Content ?? "Error occured during logging in", Severity.Error);
            }
        }
    }
}