using Learning.Application.DTOs.UserPreferences;

namespace Learning.Application.Validation.UserPreferences;

public class GetUserPreferencesValidator : BaseUserPreferencesValidator<GetUserPreferencesRequest>
{
    public GetUserPreferencesValidator()
    {
        ValidateId();
    }
}