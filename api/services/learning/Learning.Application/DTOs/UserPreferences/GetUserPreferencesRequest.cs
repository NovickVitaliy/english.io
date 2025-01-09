using Learning.Application.Validation.UserPreferences;

namespace Learning.Application.DTOs.UserPreferences;

public class GetUserPreferencesRequest : BaseUserPreferencesRequest
{
    public override bool IsValid()
    {
        return new GetUserPreferencesValidator().Validate(this).IsValid;
    }
}