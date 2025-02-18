using System.Security.Claims;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using Shared.ErrorHandling;

namespace Learning.Infrastructure.Services;

public class DecksService : IDecksService
{
    private readonly IDecksRepository _decksRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DecksService(IDecksRepository decksRepository, IHttpContextAccessor httpContextAccessor)
    {
        _decksRepository = decksRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<Guid>> CreateDeckAsync(CreateDeckRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<Guid>.BadRequest(validationResult.ErrorMessage);
        }

        var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
        var deck = new Deck
        {
            Topic = request.DeckTopic,
            DeckWords = [],
            IsStrict = request.IsStrict,
            UserEmail = userEmail
        };

        var id = await _decksRepository.CreateDeckAsync(deck);

        return Result<Guid>.Created($"api/decks/{id}", id);
    }
    public async Task<Result<GetDecksForUserResponse>> GetDecksForUser(GetDecksForUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Result<GetDecksForUserResponse>.BadRequest("Email cannot be empty");
        }

        var (decks, count) = await _decksRepository.GetDecksForUserAsync(request);

        var response = new GetDecksForUserResponse(decks.Select(x => new DeckDto(x.Id, x.UserEmail, x.Topic, x.IsStrict, x.DeckWords.Count)).ToArray(),
            count);

        return Result<GetDecksForUserResponse>.Ok(response);
    }

    public async Task<Result<DeckWithWordsDto>> GetDeckAsync(Guid deckId)
    {
        if (deckId == Guid.Empty)
        {
            return Result<DeckWithWordsDto>.BadRequest("Deck Id cannot be null");
        }

        var deck = await _decksRepository.GetDeckAsync(deckId);
        if (deck is null)
        {
            return Result<DeckWithWordsDto>.NotFound(deckId);
        }

        var dto = new DeckWithWordsDto(
            deck.Id,
            deck.UserEmail,
            deck.Topic,
            deck.IsStrict,
            deck.DeckWords.Count,
            deck.DeckWords.Select(x =>  new DeckWordDto(x.Id, x.UkrainianVersion, x.EnglishVersion, x.ExampleSentences)).ToArray());

        return Result<DeckWithWordsDto>.Ok(dto);
    }
}
