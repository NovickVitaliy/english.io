using Learning.Application.DTOs.Decks;

namespace Learning.Application.Contracts.Api;

public interface IAiLearningService
{
    Task<DeckWordDto> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences);
}
