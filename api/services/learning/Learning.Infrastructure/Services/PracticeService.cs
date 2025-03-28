using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Shared.ErrorHandling;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Infrastructure.Services;

public class PracticeService : IPracticeService
{
    private readonly IAiLearningService _aiLearningService;

    public PracticeService(IAiLearningService aiLearningService)
    {
        _aiLearningService = aiLearningService;
    }

    public async Task<Result<TranslateWordsResponse>> TranslateWords(TranslateWordsRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<TranslateWordsResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var response = await _aiLearningService.VerifyWordsTranslations(request);

        return Result<TranslateWordsResponse>.Ok(new TranslateWordsResponse(response));
    }

    public async Task<Result<SentenceWithGap[]>> GetSentencesWithGapsAsync(string[] words)
    {
        var valid = words.Length > 0 && !words.All(string.IsNullOrWhiteSpace);
        if (!valid)
        {
            return Result<SentenceWithGap[]>.BadRequest(WordsMustBePresent);
        }

        var sentencesWithGaps = await _aiLearningService.GenerateSentencesWithGaps(words);

        return Result<SentenceWithGap[]>.Ok(sentencesWithGaps);
    }
}
