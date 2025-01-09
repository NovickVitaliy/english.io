using Learning.Application.DTOs.UserPreferences;

namespace Learning.Application.Validation.UserPreferences;

public class CreateUserPreferencesValidator : BaseUserPreferencesValidator<CreateUserPreferencesRequest>
{
    public CreateUserPreferencesValidator()
    {
        ValidateUserEmail();
        ValidateNumberOfExampleSentencesPerWord();
        ValidateDailyWordPracticeLimit();
        ValidateDailySessionsReminderTimes();
    }
}