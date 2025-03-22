namespace Learning.Features.Decks.Models;

public record DeckWordDto(
    Guid Id,
    string UkrainianVersion,
    string EnglishVersion,
    string Explanation,
    string[] ExampleSentences);
