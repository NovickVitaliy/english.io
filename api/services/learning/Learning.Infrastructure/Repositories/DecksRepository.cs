using Learning.Application.Contracts.Repositories;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using MongoDB.Driver;

namespace Learning.Infrastructure.Repositories;

public class DecksRepository : IDecksRepository
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
    public async Task<Deck[]> GetDecksForUserAsync(GetDecksForUserRequest request)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.UserEmail, request.Email);

        var decks = await _learningDbContext.Decks.Find(filter)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Limit(request.PageSize)
            .Project(x => new
            {
                x.Id, x.Topic, x.IsStrict, x.UserEmail
            }).ToListAsync();

        return decks.Select(x => new Deck()
        {
            DeckWords = [],
            Id = x.Id,
            Topic = x.Topic,
            IsStrict = x.IsStrict,
            UserEmail = x.UserEmail
        }).ToArray();
    }
}
