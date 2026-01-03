using Shared.Store.Markers;

namespace Learning.Store.Decks.Actions.Fetch;

public record FetchDecksAction(string UserEmail, int PageNumber, int PageSize) : IApiAction;
