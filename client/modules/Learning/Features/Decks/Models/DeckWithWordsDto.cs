namespace Learning.Features.Decks.Models;

public record DeckWithWordsDto(
    Guid Id,
    string UserEmail,
    string Topic,
    bool IsStrict,
    int WordCount,
    DeckWordDto[] DeckWords) : DeckDto(Id, UserEmail, Topic, IsStrict, WordCount);
