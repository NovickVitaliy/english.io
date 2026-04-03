namespace Learning.Features.Practice.Models.ReadingComprehension;

public class CheckReadingComprehensionExerciseRequest
{
    public string Text { get; set; }

    public List<string> Questions { get; set; }

    public string[] Answers { get; set; }

    public CheckReadingComprehensionExerciseRequest(List<string> questions, int questionsCount, string text)
    {
        Text = text;
        Questions = questions;
        Answers = Enumerable.Range(0, questionsCount).Select(_ => "").ToArray();
    }
}
