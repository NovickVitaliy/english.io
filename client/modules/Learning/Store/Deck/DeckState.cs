using Fluxor;
using Learning.Features.Decks.Models;

namespace Learning.Store.Deck;

[FeatureState]
public record DeckState(DeckWithWordsDto? DeckWithWordsDto, bool IsLoading)
{
    private DeckState() : this(null, false)
    {

    }
}
