using Learning.Application.DTOs.Decks;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IDeckExporterService
{
    Task<Result<ExportDeckResponse>> ExportDeckAsync(ExportDeckRequest request);
}
