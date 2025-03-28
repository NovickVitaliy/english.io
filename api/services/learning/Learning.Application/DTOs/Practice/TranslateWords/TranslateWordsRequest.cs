using Shared.Requests;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslateWordsRequest(TranslatedWord[] TranslatedWords) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var result = TranslatedWords.Length > 0 && TranslatedWords.All(
            x => !string.IsNullOrWhiteSpace(x.OriginalWord)
                 && !string.IsNullOrWhiteSpace(x.OriginalLanguage)
                 && !string.IsNullOrWhiteSpace(x.TranslatedLanguage));

        return result
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, IncorrectTranslateRequest);
    }
}
