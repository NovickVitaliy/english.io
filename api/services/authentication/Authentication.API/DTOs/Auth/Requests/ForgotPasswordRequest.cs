using Authentication.API.Validators;
using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record ForgotPasswordRequest(string ResetPasswordUrl, string Email) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new ForgotPasswordRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
