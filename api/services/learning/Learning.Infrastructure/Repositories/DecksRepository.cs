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
    public async Task<(Deck[] Decks, long Count)> GetDecksForUserAsync(GetDecksForUserRequest request)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.UserEmail, request.Email);

        var decks = await _learningDbContext.Decks.Find(filter)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Limit(request.PageSize)
            .Project(x => new
            {
                x.Id, x.Topic, x.IsStrict, x.UserEmail
            }).ToListAsync();

        var count = await _learningDbContext.Decks.CountDocumentsAsync(filter);

        return (decks.Select(x => new Deck()
        {
            Id = x.Id,
            Topic = x.Topic,
            IsStrict = x.IsStrict,
            UserEmail = x.UserEmail,
        }).ToArray(), count);
    }

    public async Task<Deck?> GetDeckAsync(Guid deckId)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, deckId);

        return await (await _learningDbContext.Decks.FindAsync(filter)).SingleOrDefaultAsync();
    }

    public async Task<bool> CreateDeckEntriesAsync(Guid deckId, WordUnit wordUnit)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, deckId);
        var deck = await (await _learningDbContext.Decks.FindAsync(filter)).SingleOrDefaultAsync();
        if (deck is null)
        {
            return false;
        }

        foreach (var sense in wordUnit.Senses)
        {
            var deckEntry = new DeckEntry()
            {
                Id = Guid.NewGuid(),
                WordUnitId = wordUnit.Id,
                WordSenseId = sense.Id,
                LastTimePracticed = null,
                ProgressScore = 0
            };

            deck.DeckEntries.Add(deckEntry);
        }

        await _learningDbContext.Decks.ReplaceOneAsync(filter, deck);

        return true;
    }

    public async Task<int> GetWordsCountForDeckAsync(Guid deckId)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, deckId);
        var pipelineDefinition = new EmptyPipelineDefinition<Deck>()
            .Match(filter)
            .Project(x => x.DeckEntries.Count);

         return await (await _learningDbContext.Decks.AggregateAsync(pipelineDefinition)).SingleOrDefaultAsync();
    }

    public async Task DeleteDeckAsync(Guid deckId)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, deckId);

        await _learningDbContext.Decks.DeleteOneAsync(filter);
    }

    public async Task<bool> DeckWithNameForUserExistsAsync(string userEmail, string deckTopic)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.UserEmail, userEmail) & Builders<Deck>.Filter.Eq(x => x.Topic, deckTopic);

        var deck = await (await _learningDbContext.Decks.FindAsync(filter)).FirstOrDefaultAsync();
        return deck != null;
    }

    public async Task<bool> DeleteDeckEntryAsync(Deck deck, Guid wordId)
    {
        var deckEntry = deck.DeckEntries.SingleOrDefault(x => x.Id == wordId);
        if (deckEntry is null)
        {
            return false;
        }

        deck.DeckEntries.Remove(deckEntry);

        await _learningDbContext.Decks.ReplaceOneAsync(x => x.Id == deck.Id, deck);

        return true;
    }

    public async Task<bool> UpdateDeckAsync(Guid deckId, Deck deck)
    {
        var filter = Builders<Deck>.Filter.Eq(x => x.Id, deckId);

        await _learningDbContext.Decks.ReplaceOneAsync(filter, deck);

        return true;
    }
}
