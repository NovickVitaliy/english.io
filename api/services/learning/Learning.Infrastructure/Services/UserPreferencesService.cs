using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.UserPreferences;
using Learning.Domain;
using Shared.ErrorHandling;

namespace Learning.Infrastructure.Services;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly IUserPreferencesRepository _userPreferencesRepository;

    public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
    {
        _userPreferencesRepository = userPreferencesRepository;
    }

    public async Task<Result<Guid>> CreateUserPreferencesAsync(CreateUserPreferencesRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<Guid>.BadRequest(validationResult.ErrorMessage);
        }

        var userPreferences = new UserPreferences()
        {
            UserEmail = request.UserEmail!,
            DailySessionsReminderTimes = request.DailySessionsReminderTimes!.ToList(),
            DailyWordPracticeLimit = request.DailyWordPracticeLimit!.Value,
            NumberOfExampleSentencesPerWord = request.NumberOfExampleSentencesPerWord!.Value
        };

        var id = await _userPreferencesRepository.CreateUserPreferencesAsync(userPreferences);
        return Result<Guid>.Created($"/api/user-preferences/{id}", id);
    }

    public async Task<Result<UserPreferencesDto>> GetUserPreferencesAsync(GetUserPreferencesRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<UserPreferencesDto>.BadRequest(validationResult.ErrorMessage);
        }

        var userPreferences = await _userPreferencesRepository.GetUserPreferencesAsync(request.Id!.Value);

        return userPreferences == null
            ? Result<UserPreferencesDto>.NotFound(request.Id.Value)
            : Result<UserPreferencesDto>.Ok(new UserPreferencesDto(
                userPreferences.Id, 
                userPreferences.UserEmail, 
                userPreferences.NumberOfExampleSentencesPerWord,
                userPreferences.DailyWordPracticeLimit,
                userPreferences.DailySessionsReminderTimes));
    }

    public async Task<Result<bool>> UpdateUserPreferencesAsync(UpdateUserPreferencesRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<bool>.BadRequest(validationResult.ErrorMessage); 
        }
        
        var userPreferences = new UserPreferences()
        {
            UserEmail = request.UserEmail!,
            DailySessionsReminderTimes = request.DailySessionsReminderTimes!.ToList(),
            DailyWordPracticeLimit = request.DailyWordPracticeLimit!.Value,
            NumberOfExampleSentencesPerWord = request.NumberOfExampleSentencesPerWord!.Value
        };

        await _userPreferencesRepository.UpdateUserPreferencesAsync(request.Id!.Value, userPreferences);
        
        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> DeleteUserPreferencesAsync(DeleteUserPreferencesRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<bool>.BadRequest(validationResult.ErrorMessage);
        }

        await _userPreferencesRepository.DeleteUserPreferencesAsync(request.Id!.Value);
        
        return Result<bool>.NoContent();
    }
}