using Authentication.Features.Login.Models;
using Authentication.Features.Register.Models;
using Authentication.Shared.Models;
using Refit;

namespace Authentication.Shared.Services;

public interface IAuthenticationService
{
    const string ApiUrlKey = "Authentication";
    
    [Post("/register")]
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    [Post("/login")]
    Task<AuthResponse> LoginAsync(LoginRequest request);
}