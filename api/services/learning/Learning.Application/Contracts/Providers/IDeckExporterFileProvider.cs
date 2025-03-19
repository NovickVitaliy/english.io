using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Providers;

public interface IDeckExporterFileProvider
{
    bool Handles(ExportDeckFileType exportDeckFileType);
    Task<Stream> ExportDeckAsync(Deck deck);
}
