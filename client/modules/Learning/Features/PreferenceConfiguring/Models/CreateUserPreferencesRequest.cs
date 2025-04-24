namespace Learning.Features.PreferenceConfiguring.Models;

public class CreateUserPreferencesRequest
{
    public string UserEmail { get; set; } = null!;

    public int NumberOfExampleSentencesPerWord { get; set; }

    public int DailyWordPracticeLimit { get; set; }

    public int DailySessionsCount { get; set; }

    public List<TimeSpan> DailySessionsReminderTimes { get; set; } = new List<TimeSpan>();

    public NotificationChannel NotificationChannel { get; set; } = NotificationChannel.Email;

    public string TimezoneId { get; set; } = null!;
}
