namespace Learning.Application.DTOs.Decks;

public record DeckWithWordsDto(
    Guid Id,
    string UserEmail,
    string Topic,
    bool IsStrict,
    long WordCount,
    DeckEntryDto[] DeckWords) : DeckDto(Id, UserEmail, Topic, IsStrict, WordCount);
