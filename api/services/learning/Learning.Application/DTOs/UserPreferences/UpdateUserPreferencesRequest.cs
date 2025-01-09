using Learning.Application.Validation.UserPreferences;

namespace Learning.Application.DTOs.UserPreferences;

public class UpdateUserPreferencesRequest : BaseUserPreferencesRequest
{
    public override bool IsValid()
    {
        return new UpdateUserPreferencesValidator().Validate(this).IsValid;
    }
}