using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.Decks.Actions.Fetch;

public record FetchDecksSuccessAction(DeckDto[] Decks, long Count) : IApiCompletedAction;
