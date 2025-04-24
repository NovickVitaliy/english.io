using Learning.Application.Validation.UserPreferences;
using Learning.Domain.Models;
using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public record UpdateUserPreferencesRequest(
    Guid? Id,
    string? UserEmail,
    int? NumberOfExampleSentencesPerWord,
    int? DailyWordPracticeLimit,
    TimeSpan[]? DailySessionsReminderTimes,
    NotificationChannel NotificationChannel)
    : IBaseUserPreferencesRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new UpdateUserPreferencesValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
