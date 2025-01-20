using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;

[Route("[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public AuthenticationController()
    {
        _jwtSecurityTokenHandler = new();
    }

    public async Task<IActionResult> SignToApp(string token, string? redirectUri)
    {
        var claims = _jwtSecurityTokenHandler.ReadJwtToken(token).Claims;
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return LocalRedirect(redirectUri ?? "/learning/home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return LocalRedirect("/");
    }
}
