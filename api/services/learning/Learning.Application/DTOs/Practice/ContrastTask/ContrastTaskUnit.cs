namespace Learning.Application.DTOs.Practice.ContrastTask;

public class ContrastTaskUnit(
        Guid SenseId,
        string Sentence,
        string CorrectWord,
        string[] PossibleChoices = null!)
{
    public Guid SenseId { get; set; } = SenseId;
    public string Sentence { get; init; } = Sentence;
    public string CorrectWord { get; init; } = CorrectWord;
    public string[] PossibleChoices { get; set; } = PossibleChoices;
}
