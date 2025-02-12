using Authentication.API.Validators;
using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record RegisterUserRequest(
        string Email,
        string Password,
        string ConfirmPassword) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new RegisterUserRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
