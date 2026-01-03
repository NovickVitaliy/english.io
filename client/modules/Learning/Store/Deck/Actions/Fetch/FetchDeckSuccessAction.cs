using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Fetch;

public record FetchDeckSuccessAction(DeckWithWordsDto DeckWithWordsDto) : IApiCompletedAction;
