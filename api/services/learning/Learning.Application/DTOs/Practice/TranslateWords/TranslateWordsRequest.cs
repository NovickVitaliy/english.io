using Shared.Requests;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslateWordsRequest(TranslatedWord[] TranslatedWords, string OriginalLanguage, string TranslatedLanguage) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var result = TranslatedWords.Length > 0 && TranslatedWords.All(x => !string.IsNullOrWhiteSpace(x.OriginalWord))
            && !string.IsNullOrWhiteSpace(OriginalLanguage) && !string.IsNullOrWhiteSpace(TranslatedLanguage);

        return result
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, IncorrectTranslateRequest);
    }
}
