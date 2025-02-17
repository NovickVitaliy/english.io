using Learning.Application.Contracts.Repositories;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using MongoDB.Driver;

namespace Learning.Infrastructure.Repositories;

public class DecksRepository: IDecksRepository
{
    private readonly LearningDbContext _learningDbContext;

    public DecksRepository(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    public async Task<Guid> CreateDeckAsync(Deck deck)
    {
        await _learningDbContext.Decks.InsertOneAsync(deck);

        return deck.Id;
    }
}
