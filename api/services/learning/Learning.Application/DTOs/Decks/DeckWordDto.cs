namespace Learning.Application.DTOs.Decks;

public record DeckWordDto(
    Guid Id,
    string UkrainianVersion,
    string EnglishVersion,
    string[] ExampleSentences);
