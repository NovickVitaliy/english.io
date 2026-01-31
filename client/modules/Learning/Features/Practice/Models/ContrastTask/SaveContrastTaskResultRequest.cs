using Learning.Features.Practice.Models.GetWordsForPractice;

namespace Learning.Features.Practice.Models.ContrastTask;

public class SaveContrastTaskResultRequest
{
    public Guid DeckId { get; }
    public Dictionary<Guid, (bool IsCorrect, string Word)> AnswersMap { get; init; }

    public SaveContrastTaskResultRequest(Guid deckId, WordForPractice[] wordsForPractice)
    {
        DeckId = deckId;
        AnswersMap = wordsForPractice.ToDictionary(x => x.SenseId, _ => (false, string.Empty));
    }
}
