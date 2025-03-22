namespace Learning.Application.DTOs.Decks;

public record DeckWordDto(
    string UkrainianVersion,
    string EnglishVersion,
    string Explanation,
    string[] ExampleSentences);
