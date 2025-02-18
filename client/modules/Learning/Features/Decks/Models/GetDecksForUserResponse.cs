namespace Learning.Features.Decks.Models;

public record GetDecksForUserResponse(DeckDto[] Decks, long Count);
