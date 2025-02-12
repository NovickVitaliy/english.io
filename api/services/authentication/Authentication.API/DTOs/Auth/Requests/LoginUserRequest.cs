using Authentication.API.Validators;
using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record LoginUserRequest(
    string Email,
    string Password) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new LoginUserRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
