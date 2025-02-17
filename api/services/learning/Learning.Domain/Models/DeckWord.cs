namespace Learning.Domain.Models;

public class DeckWord
{
    public Guid Id { get; init; }
    public string UkrainianVersion { get; set; } = null!;
    public string EnglishVersion { get; set; } = null!;
    public string[] ExampleSentences { get; set; } = [];
}
