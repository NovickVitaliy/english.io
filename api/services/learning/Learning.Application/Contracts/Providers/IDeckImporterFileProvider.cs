using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Learning.Application.Contracts.Providers;

public interface IDeckImporterFileProvider
{
    bool Handles(string contentType);
    Task<Deck?> ImportDeckAsync(IFormFile file);
}
