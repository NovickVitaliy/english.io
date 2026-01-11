namespace Learning.Features.Practice.Models.TranslateWordsTask.Check;

public class CheckTranslateWordsTaskRequest
{
    public TranslatedWord[] TranslatedWords { get; set; }
    public string OriginalLanguage { get; init; }
    public string TranslateLanguage { get; init; }
    public Guid DeckId { get; init; }

    public CheckTranslateWordsTaskRequest(int wordsCount, string originalLanguage, string translateLanguage, Guid deckId)
    {
        TranslatedWords = new TranslatedWord[wordsCount].Select(_ => new TranslatedWord()).ToArray();
        OriginalLanguage = originalLanguage;
        TranslateLanguage = translateLanguage;
        DeckId = deckId;
    }
}
