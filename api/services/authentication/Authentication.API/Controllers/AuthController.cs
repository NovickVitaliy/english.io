using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Other;
using Authentication.API.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        return (await _authService.RegisterUser(request)).ToApiResponse();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        return  (await _authService.LoginUser(request)).ToApiResponse();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        return (await _authService.ForgotPasswordAsync(request)).ToApiResponse();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return (await _authService.ResetPasswordAsync(request)).ToApiResponse();
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        return (await _authService.ChangePasswordAsync(request)).ToApiResponse();
    }

    [HttpPost("send-verification-email")]
    [Authorize]
    public async Task<IActionResult> SendVerifyingEmailMessageAsync(SendVerifyingEmailMessageRequest request)
    {
        return (await _authService.SendVerifyingEmailMessageAsync(request)).ToApiResponse();
    }
}
