using Shared.Store.Markers;

namespace Learning.Store.Practice.Actions.FetchWordsForPractice;

public record GetWordsForPracticeAction(Guid DeckId) : IApiAction;
