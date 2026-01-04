using Learning.Domain.Models;

namespace Learning.Infrastructure;

public static class Constants
{
    public const float BaseLearningRate = 0.2f;

    public static float GetForgettingFactor(PracticeDifficulty practice)
    {
        return practice switch
        {
            PracticeDifficulty.VeryEasy => 1f,
            PracticeDifficulty.Easy => 0.97f,
            PracticeDifficulty.Medium => 0.95f,
            PracticeDifficulty.Hard => 0.93f,
            PracticeDifficulty.VeryHard => 0.9f,
            _ => throw new ArgumentOutOfRangeException(nameof(practice), practice, null),
        };
    }
}
