using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Learning.Controllers;

[Route("[controller]/[action]")]
public class LearningController : ControllerBase
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public LearningController()
    {
        _jwtSecurityTokenHandler = new();
    }

    public async Task<IActionResult> ConfigurePreference(string token, bool isTelegram)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException();
        }

        await HttpContext.SignOutAsync();

        var claims = _jwtSecurityTokenHandler.ReadJwtToken(token).Claims.ToList();
        claims = claims.Where(x => x.Type != GlobalConstants.ApplicationClaimTypes.PreferencesConfigured).ToList();
        claims.Add(new Claim(GlobalConstants.ApplicationClaimTypes.PreferencesConfigured, "true"));
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return isTelegram
            ? LocalRedirect("/preference-configuration/telegram-connect")
            : LocalRedirect("/learning/home");
    }
}
