using Learning.Application.Validation.UserPreferences;

namespace Learning.Application.DTOs.UserPreferences;

public class CreateUserPreferencesRequest : BaseUserPreferencesRequest
{
    public override bool IsValid()
    {
        return new CreateUserPreferencesValidator().Validate(this).IsValid;
    }
}