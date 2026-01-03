using Shared.Store.Markers;

namespace Learning.Store.DeckWords.Actions.Delete;

public record DeleteDeckEntryAction(Guid DeckId, Guid WordEntryId) : IApiAction;
