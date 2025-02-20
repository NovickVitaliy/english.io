using Learning.Application.DTOs.Decks;

namespace Learning.Application.Contracts.Api;

public interface IAiLearningService
{
    const string HttpClientKey = "AiLearningService";
    Task<DeckWordDto> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences);
}
