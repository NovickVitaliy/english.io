using Shared.Store.Markers;

namespace Learning.Store.DeckWords.Actions.Delete;

public record DeleteDeckEntrySuccessAction(Guid EntryWordId) : IApiCompletedAction;
