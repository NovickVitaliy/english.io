using Learning.Features.Practice.Models.TranslateWords;

namespace Learning.Features.Practice.Models;

public class TranslateWordsRequest
{
    public TranslatedWord[] TranslatedWords { get; set; }

    public TranslateWordsRequest(int wordsCount)
    {
        TranslatedWords = new TranslatedWord[wordsCount].Select(_ => new TranslatedWord()).ToArray();
    }
}
