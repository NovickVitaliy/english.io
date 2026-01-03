using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Microsoft.AspNetCore.Http;
using Shared.ErrorHandling;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Infrastructure.Services;

public class DecksService : IDecksService
{
    private readonly IDecksRepository _decksRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAiLearningService _aiLearningService;
    private readonly IWordUnitService _wordUnitService;

    public DecksService(IDecksRepository decksRepository, IHttpContextAccessor httpContextAccessor, IAiLearningService aiLearningService, IWordUnitService wordUnitService)
    {
        _decksRepository = decksRepository;
        _httpContextAccessor = httpContextAccessor;
        _aiLearningService = aiLearningService;
        _wordUnitService = wordUnitService;
    }

    public async Task<Result<Guid>> CreateDeckAsync(CreateDeckRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<Guid>.BadRequest(validationResult.ErrorMessage);
        }

        var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
        if (await _decksRepository.DeckWithNameForUserExistsAsync(userEmail, request.DeckTopic))
        {
            return Result<Guid>.BadRequest(DeckAlreadyExists);
        }

        var deck = new Deck
        {
            Topic = request.DeckTopic, DeckEntries = [], IsStrict = request.IsStrict, UserEmail = userEmail
        };

        var id = await _decksRepository.CreateDeckAsync(deck);

        return Result<Guid>.Created($"api/decks/{id}", id);
    }
    public async Task<Result<GetDecksForUserResponse>> GetDecksForUser(GetDecksForUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Result<GetDecksForUserResponse>.BadRequest(EmailCannotBeEmpty);
        }

        var (decks, count) = await _decksRepository.GetDecksForUserAsync(request);

        var response = new GetDecksForUserResponse(decks.Select(async x => new DeckDto(
                    x.Id,
                    x.UserEmail,
                    x.Topic,
                    x.IsStrict,
                    await _decksRepository.GetWordsCountForDeckAsync(x.Id)))
                .Select(x => x.Result)
                .ToArray(),
            count);

        return Result<GetDecksForUserResponse>.Ok(response);
    }

    public async Task<Result<DeckWithWordsDto>> GetDeckAsync(Guid deckId)
    {
        if (deckId == Guid.Empty)
        {
            return Result<DeckWithWordsDto>.BadRequest(DeckIdCannotBeEmpty);
        }

        var deck = await _decksRepository.GetDeckAsync(deckId);
        if (deck is null)
        {
            return Result<DeckWithWordsDto>.NotFound(deckId);
        }

        var wordUnits = await _wordUnitService.GetWordUnitsFromUsersDeck(deck);

        var dto = new DeckWithWordsDto(
            deck.Id,
            deck.UserEmail,
            deck.Topic,
            deck.IsStrict,
            deck.DeckEntries.Count,
            [.. wordUnits.Select(w =>
                new DeckEntryDto(
                    w.Word,
                    w.PartOfSpeech,
                    [.. w.Senses.Select(s => new WordSenseDto(s.Definition, s.UkrainianTranslation, s.UsageLabel, s.Examples))]))]);

        return Result<DeckWithWordsDto>.Ok(dto);
    }

    public async Task<Result<DeckEntryDto>> CreateDeckWordAsync(CreateDeckWordRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<DeckEntryDto>.BadRequest(validationResult.ErrorMessage);
        }
        var deck = await _decksRepository.GetDeckAsync(request.DeckId);
        if (deck is null)
        {
            return Result<DeckEntryDto>.NotFound(request.DeckId);
        }

        var wordUnitId = await _wordUnitService.GetWordUnitIdByWord(request.Word);
        if (deck.DeckEntries.Any(x => x.WordUnitId == wordUnitId))
        {
            return Result<DeckEntryDto>.BadRequest(WordAlreadyExists);
        }

        if (deck!.IsStrict && !await _aiLearningService.DoesWordComplyToTheArticle(request.Word, deck.Topic))
        {
            return Result<DeckEntryDto>.BadRequest(WordDoesNotComplyToTheTopic);
        }

        var wordUnit = await _wordUnitService.GetOrCreateWordUnit(request.Word);

        var success = await _decksRepository.CreateDeckEntriesAsync(request.DeckId, wordUnit);

        if (!success)
        {
            return Result<DeckEntryDto>.BadRequest("Error occured");
        }

        return Result<DeckEntryDto>.Ok(new DeckEntryDto(
                wordUnit.Word,
                wordUnit.PartOfSpeech,
                [.. wordUnit.Senses.Select(s => new WordSenseDto(s.Definition, s.UkrainianTranslation, s.UsageLabel, s.Examples))]));
    }
    public async Task<Result<bool>> DeleteDeckAsync(Guid deckId)
    {
        if (Guid.Empty == deckId)
        {
            return Result<bool>.BadRequest(DeckIdCannotBeEmpty);
        }

        await _decksRepository.DeleteDeckAsync(deckId);

        return Result<bool>.NoContent();
    }

    public async Task<Result<bool>> DeleteDeckEntryAsync(Guid deckId, Guid wordId)
    {
        var deck = await _decksRepository.GetDeckAsync(deckId);
        if (deck is null)
        {
            return Result<bool>.NotFound(deckId);
        }

        var deleted = await _decksRepository.DeleteDeckEntryAsync(deck, wordId);
        return deleted
            ? Result<bool>.NoContent()
            : Result<bool>.NotFound(wordId);
    }
}
