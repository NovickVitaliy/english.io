using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.Services.AuthService;
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
}