using Learning.Application.Contracts.Repositories;
using Learning.Domain;
using Learning.Infrastructure.Database;
using MongoDB.Driver;

namespace Learning.Infrastructure.Repositories;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly LearningDbContext _learningDbContext;

    public UserPreferencesRepository(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    public async Task<UserPreferences?> GetUserPreferencesAsync(Guid id)
    {
        var filter = Builders<UserPreferences>
            .Filter
            .Eq(x => x.Id, id);
        
        var userPreference = await (await _learningDbContext.UserPreferences.FindAsync(filter)).SingleOrDefaultAsync();

        return userPreference;
    }

    public async Task<Guid> CreateUserPreferencesAsync(UserPreferences userPreferences)
    {
        await _learningDbContext.UserPreferences.InsertOneAsync(userPreferences);

        return userPreferences.Id;
    }
    
    public async Task<bool> UpdateUserPreferencesAsync(Guid id, UserPreferences userPreferences)
    {
        var filter = Builders<UserPreferences>
            .Filter
            .Eq(x => x.Id, id);

        var update = Builders<UserPreferences>
            .Update
            .Set(x => x.DailySessionsReminderTimes, userPreferences.DailySessionsReminderTimes)
            .Set(x => x.DailyWordPracticeLimit, userPreferences.DailyWordPracticeLimit)
            .Set(x => x.NumberOfExampleSentencesPerWord, userPreferences.NumberOfExampleSentencesPerWord);
        
        await _learningDbContext.UserPreferences.UpdateOneAsync(filter, update);
        
        return true;
    }
    
    public async Task<bool> DeleteUserPreferencesAsync(Guid id)
    {
        var filter = Builders<UserPreferences>.Filter.Eq(x => x.Id, id);

        await _learningDbContext.UserPreferences.DeleteOneAsync(filter);

        return true;
    }
}