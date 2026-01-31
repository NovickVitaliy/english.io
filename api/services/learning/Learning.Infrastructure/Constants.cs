using Learning.Domain.Models;

namespace Learning.Infrastructure;

public static class Constants
{
    public const float BaseLearningRate = 0.2f;

    public static float GetForgettingFactor(PracticeDifficulty practiceDifficulty)
    {
        return practiceDifficulty switch
        {
            PracticeDifficulty.VeryEasy => 1f,
            PracticeDifficulty.Easy => 0.97f,
            PracticeDifficulty.Medium => 0.95f,
            PracticeDifficulty.Hard => 0.93f,
            PracticeDifficulty.VeryHard => 0.9f,
            _ => throw new ArgumentOutOfRangeException(nameof(practiceDifficulty), practiceDifficulty, null),
        };
    }

    public static float GetDifficultyMultiplier(PracticeDifficulty practiceDifficulty)
    {
        return practiceDifficulty switch
        {
            PracticeDifficulty.VeryEasy => 1.4f,
            PracticeDifficulty.Easy => 1.2f,
            PracticeDifficulty.Medium => 1f,
            PracticeDifficulty.Hard => 0.8f,
            PracticeDifficulty.VeryHard => 0.6f,
            _ => throw new ArgumentOutOfRangeException(nameof(practiceDifficulty), practiceDifficulty, null)
        };
    }

    public static float GetTaskWeight(PracticeTask practiceTask)
    {
        return practiceTask switch {
            PracticeTask.TranslateFromEnglishToUkrainian => 1f,
            PracticeTask.TranslateFromUkrainianToEnglish => 1f,
            PracticeTask.FillInTheGaps => 0.8f,
            PracticeTask.ContrastTask => 1.2f,
            _ => throw new ArgumentOutOfRangeException(nameof(practiceTask), practiceTask, null)
        };
    }
}
