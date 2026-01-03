using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Repositories;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Shared;

namespace Learning.Infrastructure.Services;

public class WordUnitService : IWordUnitService
{
    private readonly IAiLearningService _aiLearningService;
    private readonly LearningDbContext _learningDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WordUnitService(IAiLearningService aiLearningService, LearningDbContext learningDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _aiLearningService = aiLearningService;
        _learningDbContext = learningDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WordUnit> GetOrCreateWordUnit(string word)
    {
        var filter = Builders<WordUnit>.Filter.Eq(x => x.Word, word);

        var wordUnit = await (await _learningDbContext.WordUnits.FindAsync(filter)).FirstOrDefaultAsync();
        if (wordUnit is null)
        {
            int exampleSentences = int.Parse(_httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == GlobalConstants.ApplicationClaimTypes.ExampleSentencesPerWord).Value);
            wordUnit = await _aiLearningService.GetTranslatedWordWithExamplesAsync(word, exampleSentences);
            if (wordUnit is not null)
            {
                wordUnit.Id = Guid.NewGuid();
                foreach (var sense in wordUnit.Senses)
                {
                    sense.Id = Guid.NewGuid();
                }
                await _learningDbContext.WordUnits.InsertOneAsync(wordUnit!);
            }
        }

        return wordUnit!;
    }

    public async Task<Guid?> GetWordUnitIdByWord(string word)
    {
        return (await GetOrCreateWordUnit(word)).Id;
    }

    public async Task<IReadOnlyCollection<WordUnit>> GetWordUnitsFromUsersDeck(Deck deck)
    {
        var filter = Builders<WordUnit>.Filter.In(x => x.Id, deck.DeckEntries.Select(x => x.WordUnitId));

        return await (await _learningDbContext.WordUnits.FindAsync(filter)).ToListAsync();
    }
}
