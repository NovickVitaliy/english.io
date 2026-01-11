using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IDecksRepository
{
    Task<Guid> CreateDeckAsync(Deck deck);
    Task<(Deck[] Decks, long Count)> GetDecksForUserAsync(GetDecksForUserRequest request);
    Task<Deck?> GetDeckAsync(Guid deckId);
    Task<bool> CreateDeckEntriesAsync(Guid deckId, WordUnit wordUnit);
    Task<int> GetWordsCountForDeckAsync(Guid deckId);
    Task DeleteDeckAsync(Guid deckId);
    Task<bool> DeckWithNameForUserExistsAsync(string userEmail, string deckTopic);
    Task<bool> DeleteDeckEntryAsync(Deck deck, Guid wordId);
    Task<bool> UpdateDeckAsync(Guid deckId, Deck deck);
}
