using Shared.Requests;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.DTOs.Decks;

public record ExportDeckRequest(Guid DeckId, ExportDeckFileType Type = ExportDeckFileType.Csv) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        return Guid.Empty != DeckId
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, DeckIdCannotBeEmpty);
    }
}
