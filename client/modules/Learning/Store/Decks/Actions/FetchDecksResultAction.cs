using Learning.Features.Decks.Models;

namespace Learning.Store.Decks.Actions;

public record FetchDecksResultAction(DeckDto[] Decks, long Count);
