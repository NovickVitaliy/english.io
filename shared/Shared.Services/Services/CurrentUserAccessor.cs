using System.Security.Claims;
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

    public int? GetExampleSentencesPerWord()
    {
        var value = GetClaimValue(GlobalConstants.ApplicationClaimTypes.ExampleSentencesPerWord);
        return value != null
            ? int.Parse(value)
            : null;
    }

    public int? GetCountOfWordsForPractice()
    {
        var value = GetClaimValue(GlobalConstants.ApplicationClaimTypes.CountOfWordsForPractice);
        return value != null
            ? int.Parse(value)
            : null;
    }

    public string? GetNotificationChannel()
    {
        return GetClaimValue(GlobalConstants.ApplicationClaimTypes.NotificationChannel);
    }

    public bool? IsTelegramConnected()
    {
        var value = GetClaimValue(GlobalConstants.ApplicationClaimTypes.IsTelegramConnected);

        return value != null
            ? bool.Parse(value)
            : null;
    }

    public string? GetPracticeDifficulty() => GetClaimValue(GlobalConstants.ApplicationClaimTypes.PracticeDifficulty);

    public string? GetEmail() => GetClaimValue(ClaimTypes.Email);

    public string? GetName() => GetClaimValue(ClaimTypes.Name);

    public string? GetEmailVerified() => GetClaimValue("email_verified");

    public string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == claimType)?.Value;
    }
}
