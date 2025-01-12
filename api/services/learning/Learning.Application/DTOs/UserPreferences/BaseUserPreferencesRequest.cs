using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public abstract record BaseUserPreferencesRequest(
    Guid? Id,
    string? UserEmail,
    int? NumberOfExampleSentencesPerWord,
    int? DailyWordPracticeLimit,
    TimeSpan[]? DailySessionsReminderTimes) : BaseRequest;