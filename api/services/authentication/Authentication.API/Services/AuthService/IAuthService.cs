using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Auth.Responses;
using Authentication.API.DTOs.Other;
using Authentication.API.Models;
using Shared.ErrorHandling;

namespace Authentication.API.Services.AuthService;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterUser(RegisterUserRequest request);
    Task<Result<AuthResponse>> LoginUser(LoginUserRequest request);
    Task<Result<User>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<Result<User>> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest request);
    Task<Result<User>> SendVerifyingEmailMessageAsync(SendVerifyingEmailMessageRequest request);
}
