using Refit;
using Shared.Store.Markers;

namespace Learning.Store.Deck.Actions.Import;

public record ImportDeckAction(StreamPart StreamPart) : IApiAction;
