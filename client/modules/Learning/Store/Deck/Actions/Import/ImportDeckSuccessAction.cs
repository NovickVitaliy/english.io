using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Import;

public record ImportDeckSuccessAction(DeckDto Deck) : IApiCompletedAction;
