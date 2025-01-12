using Learning.Application.Validation.UserPreferences;
using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public record CreateUserPreferencesRequest(Guid? Id, string? UserEmail, int? NumberOfExampleSentencesPerWord, int? DailyWordPracticeLimit, TimeSpan[]? DailySessionsReminderTimes)
    : BaseUserPreferencesRequest(Id, UserEmail, NumberOfExampleSentencesPerWord, DailyWordPracticeLimit, DailySessionsReminderTimes)
{
    public override RequestValidationResult IsValid()
    {
        var validationResult = new CreateUserPreferencesValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}