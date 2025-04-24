using Learning.Application.Validation.UserPreferences;
using Learning.Domain.Models;
using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public record CreateUserPreferencesRequest(
    Guid? Id,
    string? UserEmail,
    int? NumberOfExampleSentencesPerWord,
    int? DailyWordPracticeLimit,
    TimeSpan[]? DailySessionsReminderTimes,
    NotificationChannel NotificationChannel,
    string? TimezoneId)
    : IBaseUserPreferencesRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new CreateUserPreferencesValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
