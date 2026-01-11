using Shared.Store.Markers;

namespace Learning.Store.Practice.Actions;

public record GetWordsForPracticeAction(Guid DeckId) : IApiAction;
