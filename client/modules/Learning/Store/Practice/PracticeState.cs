using Fluxor;

namespace Learning.Store.Practice;

[FeatureState]
public record PracticeState(string[] WordsBeingPracticed)
{
    private PracticeState() : this([])
    {

    }
}
