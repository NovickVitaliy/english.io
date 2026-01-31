using Fluxor;

namespace Learning.Store.Practice.UnknownWords;

[FeatureState]
public record UnknownWordsState(string[] UnknownWords, bool IsSaving)
{
    private UnknownWordsState() : this([], false)
    {

    }
}
