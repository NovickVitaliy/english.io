using Learning.Application.DTOs.Practice.Sessions;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IPracticeRepository
{
    Task<Guid> CreateSessionResultAsync(SessionResult sessionResult);
    Task<(IEnumerable<SessionResult> Results, long Count)> GetSessionsResultsForUserAsync(GetSessionsForUserRequest request);
}
