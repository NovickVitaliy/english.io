using Learning.Application.DTOs.Decks;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.TranslateWords;

namespace Learning.Application.Contracts.Api;

public interface IAiLearningService
{
    const string HttpClientKey = "AiLearningService";
    Task<DeckWordDto> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences);
    Task<bool> DoesWordComplyToTheArticle(string word, string topic);
    Task<TranslatedWordResult[]> VerifyWordsTranslations(TranslateWordsRequest request);
    Task<SentenceWithGap[]> GenerateSentencesWithGaps(string[] words);
    Task<string> GenerateExampleTextAsync(string[] words);
}
