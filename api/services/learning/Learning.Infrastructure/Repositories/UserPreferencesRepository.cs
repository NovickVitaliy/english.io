using Learning.Application.Contracts;
using Learning.Application.Contracts.Repositories;
using Learning.Domain;
using Learning.Infrastructure.Database;
using MongoDB.Driver;
using Shared.ErrorHandling;

namespace Learning.Infrastructure.Repositories;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly LearningDbContext _learningDbContext;

    public UserPreferencesRepository(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    public async Task<UserPreferences?> GetUserPreferencesAsync(string email)
    {
        var filter = Builders<UserPreferences>
            .Filter
            .Eq(x => x.UserEmail, email);
        
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

        await _learningDbContext.UserPreferences.ReplaceOneAsync(filter, userPreferences);
        
        return true;
    }
    
    public async Task<bool> DeleteUserPreferencesAsync(Guid id)
    {
        var filter = Builders<UserPreferences>.Filter.Eq(x => x.Id, id);

        await _learningDbContext.UserPreferences.DeleteOneAsync(filter);

        return true;
    }
}