namespace Learning.Features.PreferenceConfiguring.Models;

public class ConfigurePreferenceRequest
{
    public int NumberOfExampleSentencesPerWord { get; set; }
    
    public int DailyWordPracticeLimit { get; set; }
    
    public int DailySessionsCount { get; set; }

    public List<TimeSpan> DailySessionsReminderTimes { get; set; } = new List<TimeSpan>();

    public override string ToString()
    {
        return
            $"{nameof(NumberOfExampleSentencesPerWord)}: {NumberOfExampleSentencesPerWord}, {nameof(DailyWordPracticeLimit)}: {DailyWordPracticeLimit}, {nameof(DailySessionsCount)}: {DailySessionsCount}, {nameof(DailySessionsReminderTimes)}: {DailySessionsReminderTimes}";
    }
}