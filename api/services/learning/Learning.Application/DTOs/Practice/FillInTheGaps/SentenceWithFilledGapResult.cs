namespace Learning.Application.DTOs.Practice.FillInTheGaps;

public record SentenceWithFilledGapResult(string Sentence, string CorrectWord, Guid SenseId, bool IsCorrect);
