using Fluxor;

namespace Learning.Store.PracticeStatus;

[FeatureState]
public record PracticeStatusState(
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess)
{
    private PracticeStatusState() : this(0,0,0)
    {

    }
}
