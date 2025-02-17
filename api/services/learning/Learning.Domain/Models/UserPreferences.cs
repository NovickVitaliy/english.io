namespace Learning.Domain.Models;

public class UserPreferences
{
    public Guid Id { get; set; }

    public string UserEmail { get; set; } = null!;

    public int NumberOfExampleSentencesPerWord { get; set; }

    public int DailyWordPracticeLimit { get; set; }

    public List<TimeSpan> DailySessionsReminderTimes { get; set; } = [];
}
