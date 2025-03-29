using Fluxor;

namespace Learning.Store.Practice;

[FeatureState]
public record FIllInTheGapsState(string[] Words)
{
    private FIllInTheGapsState(): this([])
    {

    }
}
