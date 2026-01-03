using Learning.Features.Decks.Models;
using Refit;

namespace Learning.LearningShared.Services;

public interface IDecksService
{
    const string ApiUrlKey = "Learning";

    [Post("/decks")]
    Task<Guid> CreateDeckAsync(CreateDeckRequest request, [Authorize] string token);

    [Get("/decks")]
    Task<GetDecksForUserResponse> GetDecksForUserAsync(GetDecksForUserRequest request, [Authorize] string token);

    [Get("/decks/{deckId}")]
    Task<DeckWithWordsDto> GetDeckAsync(Guid deckId, [Authorize] string token);

    [Post("/decks/{deckId}/words")]
    Task<DeckEntryDto> CreateDeckWordAsync(Guid deckId, CreateDeckWordRequest request, [Authorize] string token);

    [Delete("/decks/{deckId}")]
    Task DeleteDeckAsync(Guid deckId, [Authorize] string token);

    [Get("/decks/export")]
    Task<HttpResponseMessage> ExportToFile([Query]Guid deckId, [Query] ExportFileOptions type, [Authorize] string token);

    [Delete("/decks/{deckId}/words/{wordId}")]
    Task DeleteDeckEntryAsync(Guid deckId, Guid wordId, [Authorize] string token);
}
