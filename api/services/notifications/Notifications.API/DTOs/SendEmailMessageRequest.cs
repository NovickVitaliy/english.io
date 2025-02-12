using MimeKit.Text;
using Notifications.API.Validators;
using Shared.Requests;

namespace Notifications.API.DTOs;

public record SendEmailMessageRequest(
    string ReceiverEmail,
    string ReceiverName,
    string Subject,
    TextFormat? TextFormat,
    string Body) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new SendEmailMessageRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
