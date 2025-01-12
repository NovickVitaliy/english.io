using Learning.Application.DTOs.UserPreferences;

namespace Learning.Application.Validation.UserPreferences;

public class DeleteUserPreferencesValidator : BaseUserPreferencesValidator<DeleteUserPreferencesRequest>
{
    public DeleteUserPreferencesValidator()
    {
        ValidateId();
    }
}