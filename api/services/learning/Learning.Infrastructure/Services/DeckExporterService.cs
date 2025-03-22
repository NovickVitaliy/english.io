using Learning.Application.Contracts.Providers;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Shared.ErrorHandling;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Infrastructure.Services;

public class DeckExporterService : IDeckExporterService
{
    private readonly LearningDbContext _learningDbContext;
    private readonly IEnumerable<IDeckExporterFileProvider> _deckExporterFileProviders;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeckExporterService(LearningDbContext learningDbContext, IEnumerable<IDeckExporterFileProvider> deckExporterFileProviders, IHttpContextAccessor httpContextAccessor)
    {
        _learningDbContext = learningDbContext;
        _deckExporterFileProviders = deckExporterFileProviders;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ExportDeckResponse>> ExportDeckAsync(ExportDeckRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<ExportDeckResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, request.DeckId) & Builders<Deck>.Filter.Eq(x => x.UserEmail, userEmail);
        var deck = await (await _learningDbContext.Decks.FindAsync(filter)).SingleOrDefaultAsync();
        if (deck is null)
        {
            return Result<ExportDeckResponse>.NotFound(request.DeckId);
        }


        var fileExporterProvider = _deckExporterFileProviders.FirstOrDefault(x => x.Handles(request.Type));
        if (fileExporterProvider is null)
        {
            return Result<ExportDeckResponse>.BadRequest(NoProviderForChosenFileType);
        }

        var stream = await fileExporterProvider.ExportDeckAsync(deck);
        var contentType = ExportDeckFileTypeHelper.GetHttpContentType(request.Type);
        var fileExtension = ExportDeckFileTypeHelper.GetFileExtension(request.Type);
        var fileName = $"{deck.Topic}_{(deck.IsStrict ? "strict" : "non-strict")}{fileExtension}";

        return Result<ExportDeckResponse>.Ok(new ExportDeckResponse(stream, contentType, fileName));
    }
}
