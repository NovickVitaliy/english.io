namespace Learning.Features.Practice.Models.ReadingComprehension;

public class CheckReadingComprehensionExerciseRequest
{
    public Guid DeckId { get; set; }

    public string Text { get; set; }

    public List<string> Questions { get; set; }

    public string[] Answers { get; set; }

    public CheckReadingComprehensionExerciseRequest(List<string> questions, int questionsCount, string text, Guid deckId)
    {
        DeckId = deckId;
        Text = text;
        Questions = questions;
        Answers = Enumerable.Range(0, questionsCount).Select(_ => "").ToArray();
    }
}
