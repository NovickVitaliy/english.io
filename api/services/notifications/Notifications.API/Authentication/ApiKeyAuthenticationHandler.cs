using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Notifications.API.Services.ApiKey;

namespace Notifications.API.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IApiKeyService _apiKeyService;
    private const string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyService apiKeyService) : base(options, logger, encoder)
    {
        _apiKeyService = apiKeyService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var value))
        {
            return AuthenticateResult.Fail("Api key was not provided");
        }

        var apiKey = value.ToString();

        if (!await _apiKeyService.IsApiKeyValid(apiKey))
        {
            return AuthenticateResult.Fail("Api key is not valid");
        }

        Claim[] claims = [
            new Claim(ClaimTypes.Name, "Microservice"),
            new Claim(ClaimTypes.Role, "Service")
        ];
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
