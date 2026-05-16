using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Domain.Models;
using Learning.Infrastructure.DTOs.Statistics;
using Shared.ErrorHandling;

namespace Learning.Infrastructure.Services.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IWordUnitService _wordUnitService;

    public StatisticsService(
        IStatisticsRepository statisticsRepository,
        IWordUnitService wordUnitService)
    {
        _statisticsRepository = statisticsRepository;
        _wordUnitService = wordUnitService;
    }

    public async Task RecordPracticeSessionAsync(RecordSessionStatisticsRequest request)
    {
        var now = DateTime.UtcNow;

        var snapshot = new UserStatisticsSnapshot
        {
            Id = Guid.NewGuid(),
            UserEmail = request.UserEmail,
            Date = now,
            WordsPracticed = request.WordsPracticed,
            ExercisesCompleted = 1,
            UnknownWordsMarked = request.UnknownWordsMarked,
            FillInTheGapsAccuracy = GetAccuracy(request.TaskAccuracies, PracticeTask.FillInTheGaps),
            TranslateWordsAccuracy = GetAccuracy(request.TaskAccuracies, PracticeTask.TranslateFromEnglishToUkrainian) > 0
                ? GetAccuracy(request.TaskAccuracies, PracticeTask.TranslateFromEnglishToUkrainian)
                : GetAccuracy(request.TaskAccuracies, PracticeTask.TranslateFromUkrainianToEnglish),
            ContrastTaskAccuracy = GetAccuracy(request.TaskAccuracies, PracticeTask.ContrastTask),
            ReadingComprehensionAccuracy = GetAccuracy(request.TaskAccuracies, PracticeTask.ReadingComprehension)
        };

        await _statisticsRepository.CreateSnapshotAsync(snapshot);

        var historyEntries = request.WordResults.Select(r => new WordProgressHistory
        {
            Id = Guid.NewGuid(),
            UserEmail = request.UserEmail,
            SenseId = r.SenseId,
            DeckId = request.DeckId,
            WasCorrect = r.IsCorrect,
            PracticedAt = now
        }).ToList();

        await _statisticsRepository.AddWordProgressHistoryAsync(historyEntries);

        await UpdateStreakAsync(request.UserEmail, now);
    }

    public async Task<Result<GetUserStatisticsResponse>> GetUserStatisticsAsync(string userEmail, DateTime from, DateTime to)
    {
        var snapshots = await _statisticsRepository.GetSnapshotsAsync(userEmail, from, to);
        var streak = await _statisticsRepository.GetStreakAsync(userEmail);
        var weakWords = await _statisticsRepository.GetWeakWordsAsync(userEmail, 10);

        var totalWords = snapshots.Sum(x => x.WordsPracticed);
        var totalExercises = snapshots.Sum(x => x.ExercisesCompleted);

        var accuracySnapshots = snapshots.Where(x =>
            x.FillInTheGapsAccuracy > 0 ||
            x.TranslateWordsAccuracy > 0 ||
            x.ContrastTaskAccuracy > 0).ToList();

        var overallAccuracy = accuracySnapshots.Count > 0
            ? accuracySnapshots.Average(x =>
                new[] { x.FillInTheGapsAccuracy, x.TranslateWordsAccuracy, x.ContrastTaskAccuracy }
                    .Where(a => a > 0).DefaultIfEmpty(0).Average())
            : 0f;

        var exerciseAccuracy = new ExerciseAccuracyDto(
            FillInTheGaps: snapshots.Where(x => x.FillInTheGapsAccuracy > 0).Select(x => x.FillInTheGapsAccuracy).DefaultIfEmpty(0).Average(),
            TranslateWords: snapshots.Where(x => x.TranslateWordsAccuracy > 0).Select(x => x.TranslateWordsAccuracy).DefaultIfEmpty(0).Average(),
            ContrastTask: snapshots.Where(x => x.ContrastTaskAccuracy > 0).Select(x => x.ContrastTaskAccuracy).DefaultIfEmpty(0).Average(),
            ReadingComprehension: snapshots.Where(x => x.ReadingComprehensionAccuracy > 0).Select(x => x.ReadingComprehensionAccuracy).DefaultIfEmpty(0).Average()
        );

        var dailyActivity = snapshots
            .GroupBy(x => x.Date.Date)
            .Select(g => new DailyActivityDto(
                g.Key,
                g.Sum(x => x.ExercisesCompleted),
                g.Sum(x => x.WordsPracticed)))
            .ToList();

        var weakWordDtos = new List<WeakWordDto>();
        foreach (var (senseId, wrong, correct) in weakWords)
        {
            var info = await _wordUnitService.GetWordSenseFullInfo(senseId);
            weakWordDtos.Add(new WeakWordDto(senseId, info?.EnglishWord ?? "Unknown", wrong, correct));
        }

        return Result<GetUserStatisticsResponse>.Ok(new GetUserStatisticsResponse(
            totalWords,
            totalExercises,
            streak?.CurrentStreak ?? 0,
            streak?.LongestStreak ?? 0,
            overallAccuracy,
            exerciseAccuracy,
            dailyActivity,
            weakWordDtos
        ));
    }

    public async Task<Result<GetWordProgressHistoryResponse>> GetWordProgressHistoryAsync(string userEmail, DateTime from, DateTime to)
    {
        var history = await _statisticsRepository.GetWordProgressHistoryAsync(userEmail, from, to);

        var dtos = history.Select(x => new WordProgressHistoryDto(
            x.SenseId,
            x.DeckId,
            x.Task.ToString(),
            x.WasCorrect,
            x.PracticedAt
        )).ToList();

        return Result<GetWordProgressHistoryResponse>.Ok(new GetWordProgressHistoryResponse(dtos));
    }

    private async Task UpdateStreakAsync(string userEmail, DateTime now)
    {
        var streak = await _statisticsRepository.GetStreakAsync(userEmail);
        var today = now.Date;

        if (streak is null)
        {
            await _statisticsRepository.UpsertStreakAsync(new UserStreak
            {
                Id = Guid.NewGuid(),
                UserEmail = userEmail,
                CurrentStreak = 1,
                LongestStreak = 1,
                LastPracticeDate = today
            });
            return;
        }

        var daysSinceLast = (today - streak.LastPracticeDate.Date).Days;

        if (daysSinceLast == 0) return;

        if (daysSinceLast == 1)
        {
            streak.CurrentStreak++;
        }
        else
        {
            streak.CurrentStreak = 1;
        }

        if (streak.CurrentStreak > streak.LongestStreak)
        {
            streak.LongestStreak = streak.CurrentStreak;
        }

        streak.LastPracticeDate = today;

        await _statisticsRepository.UpsertStreakAsync(streak);
    }

    private static float GetAccuracy(PracticeTaskAccuracy[] accuracies, PracticeTask task)
    {
        return accuracies.FirstOrDefault(x => x.Task == task)?.Accuracy ?? 0f;
    }
}
