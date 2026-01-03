namespace Learning.Application.DTOs.Decks;

public record DeckEntryDto(
    string Word,
    string PartOfSpeech,
    WordSenseDto[] WordSenses);
