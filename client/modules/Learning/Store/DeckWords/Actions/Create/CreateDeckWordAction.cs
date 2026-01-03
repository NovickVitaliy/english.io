using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.DeckWords.Actions.Create;

public record CreateDeckWordAction(Guid DeckId, CreateDeckWordRequest Request) : IApiAction;
