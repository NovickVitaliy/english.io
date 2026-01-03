using Fluxor;

namespace Shared.Store.Loading;

[FeatureState]
public record LoadingState(bool IsLoading)
{
    private LoadingState() : this(false)
    {

    }
}
