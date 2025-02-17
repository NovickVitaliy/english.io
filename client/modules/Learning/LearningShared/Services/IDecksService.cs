using Learning.Features.Decks.Models;
using Refit;

namespace Learning.LearningShared.Services;

public interface IDecksService
{
    const string ApiUrlKey = "Learning";

    [Post("/decks")]
    Task CreateDeckAsync(CreateDeckRequest request, [Authorize] string token);
}
