namespace Learning.Features.Practice.Models.FillInTheGaps;

public class CheckFillInTheGapsTaskRequest
{
    public Guid DeckId { get; init; }
    public SentenceWithFilledGap[] SentencesWithFilledGaps { get; init; }

    public CheckFillInTheGapsTaskRequest(Guid deckId, int wordsCount)
    {
        DeckId = deckId;
        SentencesWithFilledGaps = new SentenceWithFilledGap[wordsCount].Select(_ => new SentenceWithFilledGap()).ToArray();
    }
}
