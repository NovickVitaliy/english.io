using Authentication.Features.ForgotPassword.Models;
using Authentication.Features.Login.Models;
using Authentication.Features.Register.Models;
using Authentication.Features.ResetPassword.Models;
using Authentication.Shared.Models;
using Refit;

namespace Authentication.Shared.Services;

public interface IAuthenticationService
{
    const string ApiUrlKey = "Authentication";

    [Post("/auth/register")]
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    [Post("/auth/login")]
    Task<AuthResponse> LoginAsync(LoginRequest request);

    [Post("/auth/forgot-password")]
    Task ForgotPasswordAsync(ForgotPasswordRequest request);

    [Post("/auth/reset-password")]
    Task ResetPasswordAsync(ResetPasswordRequest request);
}
