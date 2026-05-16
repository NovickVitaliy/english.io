using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IStatisticsRepository
{
    Task CreateSnapshotAsync(UserStatisticsSnapshot snapshot);
    Task<List<UserStatisticsSnapshot>> GetSnapshotsAsync(string userEmail, DateTime from, DateTime to);
    Task<UserStreak?> GetStreakAsync(string userEmail);
    Task UpsertStreakAsync(UserStreak streak);
    Task AddWordProgressHistoryAsync(IEnumerable<WordProgressHistory> entries);
    Task<List<WordProgressHistory>> GetWordProgressHistoryAsync(string userEmail, DateTime from, DateTime to);
    Task<List<(Guid SenseId, int Wrong, int Correct)>> GetWeakWordsAsync(string userEmail, int take);
}
