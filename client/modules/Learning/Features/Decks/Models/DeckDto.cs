namespace Learning.Features.Decks.Models;

public record DeckDto(
    Guid Id,
    string UserEmail,
    string Topic,
    bool IsStrict,
    int WordCount);
