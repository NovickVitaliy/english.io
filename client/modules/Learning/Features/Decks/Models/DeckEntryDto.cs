namespace Learning.Features.Decks.Models;

public record DeckEntryDto(
    Guid Id,
    string Word,
    string PartOfSpeech,
    WordSenseDto[] WordSenses);
