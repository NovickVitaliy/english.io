using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice.GetTranslationTask;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
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
                wordUnit.AddedOn = DateTimeOffset.UtcNow;
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

    public async Task<WordForPractice[]> GetPracticeWords(List<DeckEntry> practiceWordFilters)
    {
        var senseIds = practiceWordFilters.Select(x => x.WordSenseId).ToList();
        var senseFilter = Builders<WordUnit>
            .Filter
            .ElemMatch(x => x.Senses, Builders<WordSense>.Filter.In(ws => ws.Id, senseIds));
        var wordUnits = await (await _learningDbContext.WordUnits.FindAsync(senseFilter)).ToListAsync();

        return wordUnits
            .SelectMany(x => x.Senses.Where(s => senseIds.Contains(s.Id))
                .Select(ws => new WordForPractice(x.PartOfSpeech, x.Id, x.Word, ws.Id, ws.Definition))).ToArray();
    }

    public async Task<WordSenseFullInfo?> GetWordSenseFullInfo(Guid senseId)
    {
        var filter = Builders<WordUnit>
            .Filter
            .ElemMatch(x => x.Senses, Builders<WordSense>.Filter.Eq(ws => ws.Id, senseId));

        var wordUnit = await (await _learningDbContext.WordUnits.FindAsync(filter)).SingleOrDefaultAsync();
        if (wordUnit is null)
        {
            return null;
        }

        var sense = wordUnit.Senses.Single(x => x.Id == senseId);

        return new WordSenseFullInfo(
                senseId,
                wordUnit.Word,
                sense.Definition,
                sense.UkrainianTranslation);
    }
}
