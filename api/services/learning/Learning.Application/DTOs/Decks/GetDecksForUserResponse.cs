namespace Learning.Application.DTOs.Decks;

public record GetDecksForUserResponse(
    DeckDto[] Decks,
    long Count);
