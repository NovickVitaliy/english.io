using Fluxor;
using Learning.Features.Decks.Models;

namespace Learning.Store.Decks;

[FeatureState]
public record DecksState(DeckDto[]? Decks, long Count, bool IsLoading)
{
    private DecksState() : this(null, 0, true)
    {

    }
}
