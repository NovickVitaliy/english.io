using Notifications.API.DTOs;
using Shared.ErrorHandling;

namespace Notifications.API.Services.Email;

public interface IEmailService
{
    Task<Result<bool>> SendMessageAsync(SendEmailMessageRequest request);
}
