using Refit;

namespace Learning.Features.Home.Services;

public interface IStatisticsApi
{
    const string ApiUrlKey = "Learning";

    [Get("/statistics")]
    Task<GetUserStatisticsResponse> GetStatisticsAsync(
        [Authorize] string token,
        [Query] DateTime? from = null,
        [Query] DateTime? to = null);

    [Get("/statistics/word-progress")]
    Task<GetWordProgressHistoryResponse> GetWordProgressAsync(
        [Authorize] string token,
        [Query] DateTime? from = null,
        [Query] DateTime? to = null);
}

public record GetWordProgressHistoryResponse(
    List<WordProgressHistoryDto> History
);

public record WordProgressHistoryDto(
    Guid SenseId,
    Guid DeckId,
    string Task,
    bool WasCorrect,
    DateTime PracticedAt
);

public record GetUserStatisticsResponse(
    int TotalWordsPracticed,
    int TotalExercisesCompleted,
    int CurrentStreak,
    int LongestStreak,
    float OverallAccuracy,
    ExerciseAccuracyDto ExerciseAccuracy,
    List<DailyActivityDto> DailyActivity,
    List<WeakWordDto> WeakestWords
);

public record ExerciseAccuracyDto(
    float FillInTheGaps,
    float TranslateWords,
    float ContrastTask,
    float ReadingComprehension
);

public record DailyActivityDto(
    DateTime Date,
    int ExercisesCompleted,
    int WordsPracticed
);

public record WeakWordDto(
    Guid SenseId,
    string Word,
    int TimesWrong,
    int TimesCorrect
);
