using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Shared.ErrorHandling;

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
}
