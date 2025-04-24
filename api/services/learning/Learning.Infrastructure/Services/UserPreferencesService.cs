using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.UserPreferences;
using Learning.Domain.Models;
using Learning.Infrastructure.Jobs;
using MassTransit;
using Quartz;
using Shared.ErrorHandling;
using Shared.MessageBus.Events;
using Shared.MessageBus.Requests.CreateJwtToken;

namespace Learning.Infrastructure.Services;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly IUserPreferencesRepository _userPreferencesRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IScopedClientFactory _scopedClientFactory;
    private readonly ISchedulerFactory _schedulerFactory;

    public UserPreferencesService(
        IUserPreferencesRepository userPreferencesRepository,
        IPublishEndpoint publishEndpoint,
        IScopedClientFactory scopedClientFactory, ISchedulerFactory schedulerFactory)
    {
        _userPreferencesRepository = userPreferencesRepository;
        _publishEndpoint = publishEndpoint;
        _scopedClientFactory = scopedClientFactory;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<Result<string>> CreateUserPreferencesAsync(CreateUserPreferencesRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<string>.BadRequest(validationResult.ErrorMessage);
        }

        var userPreferences = new UserPreferences()
        {
            UserEmail = request.UserEmail!,
            DailySessionsReminderTimes = request.DailySessionsReminderTimes!.ToList(),
            DailyWordPracticeLimit = request.DailyWordPracticeLimit!.Value,
            NumberOfExampleSentencesPerWord = request.NumberOfExampleSentencesPerWord!.Value,
            NotificationChannel = request.NotificationChannel
        };

        var id = await _userPreferencesRepository.CreateUserPreferencesAsync(userPreferences);
        await _publishEndpoint.Publish(new UserCreatedPreferences(request.UserEmail!, userPreferences.NumberOfExampleSentencesPerWord, userPreferences.DailyWordPracticeLimit,
            userPreferences.NotificationChannel.ToString(), false));
        await ConfigureNotificationsAsync(request.UserEmail!, request.NotificationChannel.ToString(), [.. request.DailySessionsReminderTimes!]);
        var createJwtTokenRequest = new CreateJwtTokenRequest(request.UserEmail!);
        var requestClient = _scopedClientFactory.CreateRequestClient<CreateJwtTokenRequest>();
        var jwtResponse = await requestClient.GetResponse<CreateJwtTokenResponse>(createJwtTokenRequest);
        return Result<string>.Created($"/api/user-preferences/{id}", jwtResponse.Message.JwtToken);
    }

    private async Task ConfigureNotificationsAsync(string userEmail, string notificationChannel, TimeSpan[] reminderTimes)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var job = JobBuilder.Create<NotificationJob>()
            .WithIdentity($"Notification_{userEmail}", "NotificationJobGroup")
            .UsingJobData(JobConstants.UserEmailJobDataKey, userEmail)
            .UsingJobData(JobConstants.NotificationChannelJobDataKey, notificationChannel)
            .Build();

        var triggers = reminderTimes.Select((x, i) =>
        {
            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"Notification_{userEmail}_{i}", "NotificationJobGroup")
                .WithDailyTimeIntervalSchedule(y =>
                    y.OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(x.Hours, x.Minutes))
                        .EndingDailyAfterCount(1))
                .Build();

            return trigger;
        });

        await scheduler.ScheduleJob(job, triggers.ToList(), replace: true);
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
                userPreferences.DailySessionsReminderTimes,
                userPreferences.NotificationChannel));
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
