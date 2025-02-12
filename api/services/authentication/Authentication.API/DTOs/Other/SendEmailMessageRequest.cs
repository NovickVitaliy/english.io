using Shared.Requests;

namespace Authentication.API.DTOs.Other;

public record SendEmailMessageRequest(
    string ReceiverEmail,
    string ReceiverName,
    string Subject,
    string TextFormat,
    string Body) : IBaseRequest
{
    public RequestValidationResult IsValid() => throw new NotImplementedException();
}
