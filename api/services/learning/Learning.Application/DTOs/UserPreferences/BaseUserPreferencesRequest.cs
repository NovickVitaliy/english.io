using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public interface IBaseUserPreferencesRequest : IBaseRequest
{
    Guid? Id { get; init; }
    string? UserEmail { get; init; }
    int? NumberOfExampleSentencesPerWord { get; init; }
    int? DailyWordPracticeLimit { get; init; }
    TimeSpan[]? DailySessionsReminderTimes { get; init; }
}
