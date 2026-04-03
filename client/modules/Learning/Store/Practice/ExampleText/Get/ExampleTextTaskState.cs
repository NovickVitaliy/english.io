using Fluxor;

namespace Learning.Store.Practice.ExampleText.Get;

[FeatureState]
public record ExampleTextTaskState(string Text, bool IsLoading)
{
    private ExampleTextTaskState() : this(string.Empty, false)
    {

    }
}
