using Learning.Application.Contracts.Repositories;
using Learning.Application.DTOs.Practice.Sessions;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using MongoDB.Driver;

namespace Learning.Infrastructure.Repositories;

public class PracticeRepository : IPracticeRepository
{
    private readonly LearningDbContext _db;

    public PracticeRepository(LearningDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> CreateSessionResultAsync(SessionResult sessionResult)
    {
        await _db.SessionResults.InsertOneAsync(sessionResult);

        return sessionResult.Id;
    }

    public async Task<(IEnumerable<SessionResult> Results, long Count)> GetSessionsResultsForUserAsync(GetSessionsForUserRequest request)
    {
        var filter = Builders<SessionResult>.Filter.Eq(x => x.UserEmail, request.UserEmail);
        var order = Builders<SessionResult>.Sort.Descending(x => x.PracticeDate);

        var results = await _db.SessionResults.Find(filter)
            .Sort(order)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync();

        var count = await _db.SessionResults.CountDocumentsAsync(filter);

        return (results, count);
    }
}
