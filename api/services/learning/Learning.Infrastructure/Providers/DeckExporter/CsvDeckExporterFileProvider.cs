using System.Globalization;
using CsvHelper;
using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.CsvMappers;
using Learning.Infrastructure.Database;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class CsvDeckExporterFileProvider : BaseDeckExporterProvider, IDeckExporterFileProvider
{
    private const ExportDeckFileType HandlesFileType = ExportDeckFileType.Csv;

    public CsvDeckExporterFileProvider(LearningDbContext learningDbContext) : base(learningDbContext)
    { }

    public bool Handles(ExportDeckFileType exportDeckFileType) => HandlesFileType == exportDeckFileType;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        var stream = new MemoryStream();
        try
        {
            await using var streamWriter = new StreamWriter(stream, leaveOpen: true);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<WordSenseCsvMap>();
            var wordEntries = await LoadWordsFromDatabase(deck);
            await csvWriter.WriteRecordsAsync(FlattenToWordSenses(wordEntries));
            await streamWriter.FlushAsync();
            stream.Position = 0;
            return stream;
        }
        catch (Exception)
        {
            await stream.DisposeAsync();
            throw;
        }
    }
}
