namespace Learning.Domain.Models;

public class WordSense
{
    public Guid Id { get; set; }
    public string Definition { get; init; } = null!;
    public string UkrainianTranslation { get; init; } = null!;
    public string UsageLabel { get; init; } = null!;
    public string[] Examples { get; init; } = [];
    public string[] Synonyms { get; init; } = [];
}
