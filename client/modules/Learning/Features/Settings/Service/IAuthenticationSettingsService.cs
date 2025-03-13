using Learning.Features.Settings.Models;
using Learning.Features.Settings.Models.Security;
using Refit;

namespace Learning.Features.Settings.Service;

public interface IAuthenticationSettingsService
{
    public const string ApiUrlKey = "Authentication";

    [Post("/auth/change-password")]
    Task ChangePasswordAsync(ChangePasswordRequest request, [Authorize] string token);

    [Post("/auth/send-verification-email")]
    Task SendVerifyingEmailMessageAsync(SendVerifyingEmailMessageRequest request, [Authorize] string token);
}
