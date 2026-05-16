using Learning.Domain.Models;
using Learning.Infrastructure.DTOs;
using Learning.Infrastructure.DTOs.Statistics;
using Shared.ErrorHandling;

namespace Learning.Infrastructure.Services.Statistics;

public interface IStatisticsService
{
    Task RecordPracticeSessionAsync(RecordSessionStatisticsRequest request);
    Task<Result<GetUserStatisticsResponse>> GetUserStatisticsAsync(string userEmail, DateTime from, DateTime to);
    Task<Result<GetWordProgressHistoryResponse>> GetWordProgressHistoryAsync(string userEmail, DateTime from, DateTime to);
}

public record RecordSessionStatisticsRequest(
    string UserEmail,
    Guid DeckId,
    int WordsPracticed,
    PracticeTaskAccuracy[] TaskAccuracies,
    WordSenseTaskResult[] WordResults,
    int UnknownWordsMarked
);

public record PracticeTaskAccuracy(PracticeTask Task, float Accuracy);
