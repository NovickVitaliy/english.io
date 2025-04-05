using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IPracticeRepository
{
    Task<Guid> CreateSessionResultAsync(SessionResult sessionResult);
}
