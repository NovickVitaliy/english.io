using Microsoft.AspNetCore.Http;
using Shared.Services.Contracts;

namespace Shared.Services.Services;

public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetEmail() => _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
}
