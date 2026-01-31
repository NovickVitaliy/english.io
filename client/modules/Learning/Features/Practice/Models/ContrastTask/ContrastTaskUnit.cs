namespace Learning.Features.Practice.Models.ContrastTask;

public record ContrastTaskUnit(Guid SenseId,
    string Sentence,
    string CorrectWord,
    string[] PossibleChoices);
