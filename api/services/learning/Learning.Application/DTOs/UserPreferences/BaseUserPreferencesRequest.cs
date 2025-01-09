using Shared.Requests;

namespace Learning.Application.DTOs.UserPreferences;

public abstract class BaseUserPreferencesRequest : BaseRequest
{
    public Guid? Id { get; init; }
    
    public string? UserEmail { get; init; } = null!;
    
    public int? NumberOfExampleSentencesPerWord { get; init; }
    
    public int? DailyWordPracticeLimit { get; init; }
    
    public TimeSpan[]? DailySessionsReminderTimes { get; init; } = null!;
}