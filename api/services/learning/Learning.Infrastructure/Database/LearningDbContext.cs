using Learning.Domain;
using Learning.Domain.Models;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Learning.Infrastructure.Database;

public class LearningDbContext
{
    private const string UserPreferencesCollectionName = "PreferencesCollection";
    private const string DecksCollectionName = "DecksCollection";
    private const string SessionResultsCollectionName = "SessionResults";
    private readonly IMongoDatabase _mongoDatabase;

    public LearningDbContext(IMongoClient mongoClient, IOptions<MongoOptions> mongoOptions)
    {
        _mongoDatabase = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName);
    }

    public IMongoCollection<UserPreferences> UserPreferences => _mongoDatabase.GetCollection<UserPreferences>(UserPreferencesCollectionName);
    public IMongoCollection<Deck> Decks => _mongoDatabase.GetCollection<Deck>(DecksCollectionName);
    public IMongoCollection<SessionResult> SessionResults => _mongoDatabase.GetCollection<SessionResult>(SessionResultsCollectionName);
}
