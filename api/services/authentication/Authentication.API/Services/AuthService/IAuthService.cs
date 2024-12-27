using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Auth.Responses;
using Shared.ErrorHandling;

namespace Authentication.API.Services.AuthService;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterUser(RegisterUserRequest request);
    Task<Result<AuthResponse>> LoginUser(LoginUserRequest request);
}