using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IDecksRepository
{
    Task<Guid> CreateDeckAsync(Deck deck);
    Task<(Deck[] Decks, long Count)> GetDecksForUserAsync(GetDecksForUserRequest request);
    Task<Deck?> GetDeckAsync(Guid deckId);
    Task<DeckWord?> CreateDeckWordAsync(Guid deckId, DeckWord deckWord);
}
