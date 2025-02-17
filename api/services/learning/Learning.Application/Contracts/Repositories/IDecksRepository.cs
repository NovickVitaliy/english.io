using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IDecksRepository
{
    Task<Guid> CreateDeckAsync(Deck deck);
    Task<Deck[]> GetDecksForUserAsync(GetDecksForUserRequest request);
}
