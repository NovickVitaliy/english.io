using Learning.Domain.Models;

namespace Learning.Application.DTOs.UserPreferences;

public record UserPreferencesDto(
    Guid Id,
    string UserEmail,
    int NumberOfExampleSentencesPerWord,
    int DailyWordPracticeLimit,
    List<TimeSpan> DailySessionsReminderTimes,
    NotificationChannel NotificationChannel);
