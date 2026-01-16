namespace Learning.Features.Practice.Models.FillInTheGaps;

public class SentenceWithFilledGap
{
    public string Sentence { get; set; } = null!;
    public string CorrectWord { get; set; } = null!;
    public string? FilledInWord { get; set; }
    public Guid SenseId { get; set; }
}
