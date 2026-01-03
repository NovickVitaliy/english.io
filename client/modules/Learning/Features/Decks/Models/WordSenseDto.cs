namespace Learning.Features.Decks.Models;

public record WordSenseDto(
    string Definition,
    string UkrainianTranslation,
    string UsageLabel,
    string[] ExampleSentences);
