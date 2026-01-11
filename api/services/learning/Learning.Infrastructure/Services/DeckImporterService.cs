using Learning.Application.Contracts.Providers;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Decks;
using Learning.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Shared.ErrorHandling;
using Shared.Services.Contracts;

namespace Learning.Infrastructure.Services;

public class DeckImporterService : IDeckImporterService
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IEnumerable<IDeckImporterFileProvider> _deckImporterFileProviders;
    private readonly LearningDbContext _learningDbContext;

    public DeckImporterService(
        ICurrentUserAccessor currentUserAccessor,
        IEnumerable<IDeckImporterFileProvider> deckImporterFileProviders,
        LearningDbContext learningDbContext)
    {
        _currentUserAccessor = currentUserAccessor;
        _deckImporterFileProviders = deckImporterFileProviders;
        _learningDbContext = learningDbContext;
    }

    public async Task<Result<DeckDto>> ImportDeckAsync(IFormFile file)
    {
        var userEmail = _currentUserAccessor.GetEmail();
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return Result<DeckDto>.BadRequest("Cannot obtain users email");
        }

        var importerFileProvider = _deckImporterFileProviders.SingleOrDefault(x => x.Handles(file.ContentType));
        if (importerFileProvider is null)
        {
            return Result<DeckDto>.BadRequest("Cannot import file with the given extension");
        }

        var deck = await importerFileProvider.ImportDeckAsync(file);
        if (deck is null)
        {
            return Result<DeckDto>.BadRequest("Error importing the deck");
        }

        deck.UserEmail = userEmail;

        await _learningDbContext.Decks.InsertOneAsync(deck);

        return Result<DeckDto>.Created($"/api/decks/{deck.Id}", new DeckDto(
                deck.Id,
                deck.UserEmail,
                deck.Topic,
                deck.IsStrict,
                deck.DeckWordsCount));
    }
}
