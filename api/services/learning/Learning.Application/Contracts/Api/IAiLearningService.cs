using Learning.Application.DTOs.Decks;
using Learning.Application.DTOs.Practice;

namespace Learning.Application.Contracts.Api;

public interface IAiLearningService
{
    const string HttpClientKey = "AiLearningService";
    Task<DeckWordDto> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences);
    Task<bool> DoesWordComplyToTheArticle(string word, string topic);
    Task<TranslatedWordResult[]> VerifyWordsTranslations(TranslateWordsRequest request);
}
