namespace Learning.Infrastructure.DTOs.Statistics;

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
