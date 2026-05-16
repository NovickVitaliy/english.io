namespace Learning.Domain.Models;

public class UserStatisticsSnapshot
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public int WordsPracticed { get; set; }
    public int ExercisesCompleted { get; set; }

    public float FillInTheGapsAccuracy { get; set; }
    public float TranslateWordsAccuracy { get; set; }
    public float ContrastTaskAccuracy { get; set; }
    public float ReadingComprehensionAccuracy { get; set; }

    public int UnknownWordsMarked { get; set; }
    public int WordsSavedToDeck { get; set; }
}
