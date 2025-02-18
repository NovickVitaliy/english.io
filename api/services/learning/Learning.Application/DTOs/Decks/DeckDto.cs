namespace Learning.Application.DTOs.Decks;

public record DeckDto(
        Guid Id,
        string UserEmail,
        string Topic,
        bool IsStrict,
        int WordCount);
