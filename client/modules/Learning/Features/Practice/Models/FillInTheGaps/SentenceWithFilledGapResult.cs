namespace Learning.Features.Practice.Models.FillInTheGaps;

public record SentenceWithFilledGapResult(string Sentence, string CorrectWord, Guid SenseId, bool IsCorrect);
