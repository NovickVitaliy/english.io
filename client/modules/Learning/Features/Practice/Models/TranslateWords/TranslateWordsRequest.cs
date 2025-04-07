using Learning.Features.Practice.Models.TranslateWords;

namespace Learning.Features.Practice.Models;

public class TranslateWordsRequest
{
    public TranslatedWord[] TranslatedWords { get; set; }
    public string OriginalLanguage { get; init; }
    public string TranslatedLanguage { get; init; }

    public TranslateWordsRequest(int wordsCount, string originalLanguage, string translatedLanguage)
    {
        TranslatedWords = new TranslatedWord[wordsCount].Select(_ => new TranslatedWord()).ToArray();
        OriginalLanguage = originalLanguage;
        TranslatedLanguage = translatedLanguage;
    }
}
