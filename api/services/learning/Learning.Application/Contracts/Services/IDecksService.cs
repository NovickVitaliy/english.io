using Learning.Application.DTOs.Decks;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IDecksService
{
    Task<Result<Guid>> CreateDeckAsync(CreateDeckRequest request);
    Task<Result<GetDecksForUserResponse>> GetDecksForUser(GetDecksForUserRequest request);
    Task<Result<DeckWithWordsDto>> GetDeckAsync(Guid deckId);
    Task<Result<DeckWordDto>> CreateDeckWordAsync(CreateDeckWordRequest request);
}
