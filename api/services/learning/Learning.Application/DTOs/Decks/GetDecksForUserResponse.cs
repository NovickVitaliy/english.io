namespace Learning.Application.DTOs.Decks;

public record GetDecksForUserResponse(
    DeckDto[] Decks,
    int PageNumber,
    int PageSize);
