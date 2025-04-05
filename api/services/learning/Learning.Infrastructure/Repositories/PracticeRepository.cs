using Learning.Application.Contracts.Repositories;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;

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
}
