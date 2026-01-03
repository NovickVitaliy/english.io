using Learning.Domain.Models;
using Learning.Infrastructure.CsvMappers;
using Learning.Infrastructure.Database;
using MongoDB.Driver;

namespace Learning.Infrastructure.Providers.DeckExporter;

public abstract class BaseDeckExporterProvider
{
    private readonly LearningDbContext _learningDbContext;

    protected BaseDeckExporterProvider(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    protected async Task<IReadOnlyCollection<WordUnit>> LoadWordsFromDatabase(Deck deck)
    {
        var wordUnitIds = deck.DeckEntries.Select(x => x.WordUnitId);

        var filter = Builders<WordUnit>.Filter.In(x => x.Id, wordUnitIds);

        return await (await _learningDbContext.WordUnits.FindAsync(filter)).ToListAsync();
    }

    // need to come up with the better method name lol
    protected static WordForExport[] FlattenToWordSenses(IEnumerable<WordUnit> wordsUnits)
    {
        return [.. wordsUnits.SelectMany(w => w.Senses.Select(s => new WordForExport(
            w.Word,
            w.PartOfSpeech,
            s.Definition,
            s.UkrainianTranslation,
            s.UsageLabel,
            string.Join('|', s.Examples))))];
    }
}
