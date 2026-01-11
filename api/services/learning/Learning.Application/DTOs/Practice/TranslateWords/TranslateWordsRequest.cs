using Shared.Requests;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslateWordsRequest(TranslatedWord[] TranslatedWords, string OriginalLanguage, string TranslateLanguage, Guid DeckId) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var result = TranslatedWords.Length > 0 && TranslatedWords.All(x => !string.IsNullOrWhiteSpace(x.OriginalWord))
            && !string.IsNullOrWhiteSpace(OriginalLanguage) && !string.IsNullOrWhiteSpace(TranslateLanguage) && DeckId != Guid.Empty;

        return result
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, IncorrectTranslateRequest);
    }
}
