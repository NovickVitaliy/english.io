using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Delete;

public record DeleteDeckSuccessAction(Guid DeckId) : IApiCompletedAction;
