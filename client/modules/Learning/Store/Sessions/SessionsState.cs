using Fluxor;
using Learning.Features.Settings.Models.Sessions;

namespace Learning.Store.Sessions;

[FeatureState]
public record SessionsState(SessionDto[]? Sessions, long Count, bool IsLoading)
{
    private SessionsState() : this([], 0, true)
    {

    }
}
