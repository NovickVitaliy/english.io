using Learning.Application.DTOs.UserPreferences;

namespace Learning.Application.Validation.UserPreferences;

public class UpdateUserPreferencesValidator : BaseUserPreferencesValidator<UpdateUserPreferencesRequest>
{
    public UpdateUserPreferencesValidator()
    {
        ValidateNumberOfExampleSentencesPerWord();
        ValidateDailyWordPracticeLimit();
        ValidateDailySessionsReminderTimes();
    }
}