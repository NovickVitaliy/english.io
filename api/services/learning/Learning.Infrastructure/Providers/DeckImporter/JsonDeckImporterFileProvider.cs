using System.Net.Mime;
using System.Text.Json;
using Learning.Application.Contracts.Providers;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Providers.DeckExporter.JsonExporter;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Learning.Infrastructure.Providers.DeckImporter;

public class JsonDeckImporterFileProvider : IDeckImporterFileProvider
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly LearningDbContext _learningDbContext;

    public JsonDeckImporterFileProvider(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    public bool Handles(string contentType) => contentType.Equals(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase);

    public async Task<Deck?> ImportDeckAsync(IFormFile file)
    {
        var importedDeck = await JsonSerializer.DeserializeAsync<DeckForJsonImportExport>(file.OpenReadStream(), Options);
        if (importedDeck is null)
        {
            return null;
        }

        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            Topic = importedDeck.Topic,
            IsStrict = importedDeck.IsStrict
        };

        foreach (var wu in importedDeck.WordUnits)
        {
            var filter = Builders<WordUnit>.Filter.Eq(x => x.Word, wu.Word);
            var item = await (await _learningDbContext.WordUnits.FindAsync(filter)).SingleOrDefaultAsync();
            if (item is null)
            {
                await _learningDbContext.WordUnits.InsertOneAsync(wu);
            }

            deck.DeckEntries.AddRange(wu.Senses.Select(s => new DeckEntry()
            {
                Id = Guid.NewGuid(),
                LastTimePracticed = null,
                ProgressScore = 0f,
                WordSenseId = s.Id,
                WordUnitId = wu.Id
            }));
        }

        deck.DeckWordsCount = deck.DeckEntries.Count;

        return deck;
    }
}
