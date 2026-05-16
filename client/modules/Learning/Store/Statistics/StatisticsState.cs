using Fluxor;
using Learning.Features.Home.Services;

namespace Learning.Store.Statistics;

[FeatureState]
public record StatisticsState
{
    public bool IsLoading { get; init; }
    public GetUserStatisticsResponse? Statistics { get; init; }
    public string? ErrorMessage { get; init; }
}
