namespace Learning.Domain.Models;

public class WordUnit
{
    public Guid Id { get; set; }
    public string Word { get; init; } = null!;
    public string PartOfSpeech { get; init; } = null!;
    public WordSense[] Senses { get; init; } = [];
    public DateTimeOffset AddedOn { get; set; }
}
