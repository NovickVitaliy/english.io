using Learning.Features.Decks.Models;
using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Create;

public record CreateDeckAction(CreateDeckRequest Request) : IApiAction;
