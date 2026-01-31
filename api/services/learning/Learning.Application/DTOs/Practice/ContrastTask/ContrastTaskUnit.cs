namespace Learning.Application.DTOs.Practice.ContrastTask;

public record ContrastTaskUnit(
        Guid SenseId,
        string Sentence,
        string CorrectWord,
        string[] PossibleChoices = null!);
