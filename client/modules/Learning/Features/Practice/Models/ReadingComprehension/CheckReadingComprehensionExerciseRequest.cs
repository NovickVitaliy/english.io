namespace Learning.Features.Practice.Models.ReadingComprehension;

public class CheckReadingComprehensionExerciseRequest
{
    public string Text { get; set; }

    public string[] Questions { get; set; }

    public string[] Answers { get; set; }

    public CheckReadingComprehensionExerciseRequest(string[] questions, string text)
    {
        Text = text;
        Questions = questions;
        Answers = Enumerable.Range(0, Questions.Length).Select(_ => "").ToArray();
    }
}
