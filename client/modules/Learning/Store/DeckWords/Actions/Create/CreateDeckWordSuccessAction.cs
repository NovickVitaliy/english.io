using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.DeckWords.Actions;

public record CreateDeckWordSuccessAction(DeckEntryDto DeckEntryDto) : IApiCompletedAction;
