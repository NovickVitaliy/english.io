using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record VerifyEmailRequest(string Token) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        return string.IsNullOrWhiteSpace(Token)
            ? new RequestValidationResult(false, "Token is null")
            : new RequestValidationResult(true);
    }
}
