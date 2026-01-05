using Learning.Application.DTOs.Decks;
using Microsoft.AspNetCore.Http;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IDeckImporterService
{
    Task<Result<DeckDto>> ImportDeckAsync(IFormFile file);
}
