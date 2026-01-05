using System.Text.Encodings.Web;
using System.Text.Json;
using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class JsonDeckExporterFileProvider : BaseDeckExporterProvider, IDeckExporterFileProvider
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public JsonDeckExporterFileProvider(LearningDbContext learningDbContext) : base(learningDbContext)
    { }

    public bool Handles(ExportDeckFileType exportDeckFileType) => exportDeckFileType == ExportDeckFileType.Json;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        var stream = new MemoryStream();
        var wordEntries = LoadWordsFromDatabase(deck);
        await JsonSerializer.SerializeAsync(stream, wordEntries, Options);
        stream.Position = 0;

        return stream;
    }
}
