namespace Learning.Application.DTOs.Decks;

public record WordSenseDto(
    string Definition,
    string UkrainianTranslation,
    string UsageLabel,
    string[] ExampleSentences);
