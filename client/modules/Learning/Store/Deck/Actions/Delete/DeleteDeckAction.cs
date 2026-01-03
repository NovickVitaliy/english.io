using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Delete;

public record DeleteDeckAction(Guid DeckId) : IApiAction;
