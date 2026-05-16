using Learning.Features.Home.Services;

namespace Learning.Store.Statistics;

public record LoadStatisticsAction(DateTime? From = null, DateTime? To = null);
public record LoadStatisticsSuccessAction(GetUserStatisticsResponse Statistics);
public record LoadStatisticsFailureAction(string ErrorMessage);
