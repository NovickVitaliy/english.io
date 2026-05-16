using Learning.Application.Contracts.Repositories;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Database.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly StatisticsDbContext _db;

    public StatisticsRepository(StatisticsDbContext db)
    {
        _db = db;
    }

    public async Task CreateSnapshotAsync(UserStatisticsSnapshot snapshot)
    {
        await _db.UserStatisticsSnapshots.AddAsync(snapshot);
        await _db.SaveChangesAsync();
    }

    public async Task<List<UserStatisticsSnapshot>> GetSnapshotsAsync(string userEmail, DateTime from, DateTime to)
    {
        return await _db.UserStatisticsSnapshots
            .Where(x => x.UserEmail == userEmail && x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<UserStreak?> GetStreakAsync(string userEmail)
    {
        return await _db.UserStreaks
            .FirstOrDefaultAsync(x => x.UserEmail == userEmail);
    }

    public async Task UpsertStreakAsync(UserStreak streak)
    {
        var existing = await _db.UserStreaks
            .FirstOrDefaultAsync(x => x.UserEmail == streak.UserEmail);

        if (existing is null)
        {
            await _db.UserStreaks.AddAsync(streak);
        }
        else
        {
            existing.CurrentStreak = streak.CurrentStreak;
            existing.LongestStreak = streak.LongestStreak;
            existing.LastPracticeDate = streak.LastPracticeDate;
        }

        await _db.SaveChangesAsync();
    }

    public async Task AddWordProgressHistoryAsync(IEnumerable<WordProgressHistory> entries)
    {
        await _db.WordProgressHistories.AddRangeAsync(entries);
        await _db.SaveChangesAsync();
    }

    public async Task<List<WordProgressHistory>> GetWordProgressHistoryAsync(string userEmail, DateTime from, DateTime to)
    {
        return await _db.WordProgressHistories
            .Where(x => x.UserEmail == userEmail && x.PracticedAt >= from && x.PracticedAt <= to)
            .OrderBy(x => x.PracticedAt)
            .ToListAsync();
    }

    public async Task<List<(Guid SenseId, int Wrong, int Correct)>> GetWeakWordsAsync(string userEmail, int take)
    {
        return await _db.WordProgressHistories
            .Where(x => x.UserEmail == userEmail)
            .GroupBy(x => x.SenseId)
            .Select(g => new
            {
                SenseId = g.Key,
                Wrong = g.Count(x => !x.WasCorrect),
                Correct = g.Count(x => x.WasCorrect)
            })
            .OrderByDescending(x => x.Wrong)
            .Take(take)
            .ToListAsync()
            .ContinueWith(t => t.Result
                .Select(x => (x.SenseId, x.Wrong, x.Correct))
                .ToList());
    }
}
