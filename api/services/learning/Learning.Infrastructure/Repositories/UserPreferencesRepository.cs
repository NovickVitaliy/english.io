using Learning.Application.Contracts;
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

    public async Task<Result<UserPreferences>> GetUserPreferencesAsync(string email)
    {
        var filter = Builders<UserPreferences>
            .Filter
            .Eq(x => x.UserEmail, email);
        
        var userPreference = await (await _learningDbContext.UserPreferences.FindAsync(filter)).SingleOrDefaultAsync();

        return userPreference == null
            ? Result<UserPreferences>.NotFound(email)
            : Result<UserPreferences>.Ok(userPreference);
    }

    public async Task<Result<Guid>> CreateUserPreferencesAsync(UserPreferences userPreferences)
    {
        await _learningDbContext.UserPreferences.InsertOneAsync(userPreferences);
        
        return Result<Guid>.Created($"api/user-preferences/{userPreferences.Id}", userPreferences.Id);
    }
    
    public async Task<Result<bool>> UpdateUserPreferencesAsync(Guid id, UserPreferences userPreferences)
    {
        var filter = Builders<UserPreferences>
            .Filter
            .Eq(x => x.Id, id);

        await _learningDbContext.UserPreferences.ReplaceOneAsync(filter, userPreferences);
        
        return Result<bool>.Ok(true);
    }
    
    public async Task<Result<bool>> DeleteUserPreferencesAsync(Guid id)
    {
        var filter = Builders<UserPreferences>.Filter.Eq(x => x.Id, id);

        await _learningDbContext.UserPreferences.DeleteOneAsync(filter);
        
        return Result<bool>.NoContent();
    }
}