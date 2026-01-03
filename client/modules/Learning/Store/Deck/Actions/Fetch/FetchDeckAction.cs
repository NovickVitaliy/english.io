using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Fetch;

public record FetchDeckAction(Guid DeckId) : IApiAction;
