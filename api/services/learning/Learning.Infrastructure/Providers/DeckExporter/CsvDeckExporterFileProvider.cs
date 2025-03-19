using System.Globalization;
using CsvHelper;
using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.CsvMappers;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class CsvDeckExporterFileProvider : IDeckExporterFileProvider
{
    private readonly ExportDeckFileType _handlesFileType = ExportDeckFileType.Csv;

    public bool Handles(ExportDeckFileType exportDeckFileType) => _handlesFileType == exportDeckFileType;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        var stream = new MemoryStream();
        try
        {
            await using var streamWriter = new StreamWriter(stream, leaveOpen: true);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<DeckWordMap>();
            await csvWriter.WriteRecordsAsync(deck.DeckWords);
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
