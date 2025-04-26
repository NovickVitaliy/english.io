namespace Learning.Features.Practice.Models.ReadingComprehension;

public record CheckReadingComprehensionExerciseResult(int AnswersCorrect, CheckAnswerResult[] AnswersResults);

public record CheckAnswerResult(bool IsCorrect, string CorrectAnswer);
