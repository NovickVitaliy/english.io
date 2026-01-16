namespace Learning.Application.DTOs.Practice.FillInTheGaps;

public record SentenceWithFilledGap(
        string Sentence,
        string CorrectWord,
        string? FilledInWord,
        Guid SenseId);
