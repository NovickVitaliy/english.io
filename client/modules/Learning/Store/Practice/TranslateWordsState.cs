using Fluxor;

namespace Learning.Store.Practice;

[FeatureState]
public record TranslateWordsState(string[] WordsBeingPracticed)
{
    private TranslateWordsState() : this([])
    {

    }
}
