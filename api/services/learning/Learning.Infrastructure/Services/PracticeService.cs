using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ExampleText;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Shared.ErrorHandling;
using Shared.Services.Contracts;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Infrastructure.Services;

public class PracticeService : IPracticeService
{
    private readonly IAiLearningService _aiLearningService;
    private readonly IPracticeRepository _practiceRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public PracticeService(IAiLearningService aiLearningService, IPracticeRepository practiceRepository, ICurrentUserAccessor currentUserAccessor)
    {
        _aiLearningService = aiLearningService;
        _practiceRepository = practiceRepository;
        _currentUserAccessor = currentUserAccessor;
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
    public async Task<Result<GetExampleTextResponse>> GetExampleTextAsync(string[] words)
    {
        var isValid = words.Length > 0 && !words.All(string.IsNullOrWhiteSpace);
        if (!isValid)
        {
            return Result<GetExampleTextResponse>.BadRequest(WordsMustBePresent);
        }

        var result = await _aiLearningService.GenerateExampleTextAsync(words);

        return Result<GetExampleTextResponse>.Ok(new GetExampleTextResponse(result));
    }

    public async Task<Result<SaveSessionResultDto>> SaveSessionResultAsync(SaveSessionResultRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<SaveSessionResultDto>.BadRequest(validationResult.ErrorMessage);
        }

        var sessionResult = new SessionResult()
        {
            Words = request.Words,
            UserEmail = _currentUserAccessor.GetEmail()!,
            FirstTaskPercentageSuccess = request.FirstTaskPercentageSuccess,
            SecondTaskPercentageSuccess = request.SecondTaskPercentageSuccess,
            ThirdTaskPercentageSuccess = request.ThirdTaskPercentageSuccess,
            FourthTaskPercentageSuccess = request.FourthTaskPercentageSuccess,
            PracticeDate = DateTime.UtcNow
        };

        var id = await _practiceRepository.CreateSessionResultAsync(sessionResult);

        var dto = new SaveSessionResultDto(sessionResult.Words, sessionResult.FirstTaskPercentageSuccess, sessionResult.SecondTaskPercentageSuccess,
            sessionResult.ThirdTaskPercentageSuccess, sessionResult.FourthTaskPercentageSuccess, sessionResult.PracticeDate);
        return Result<SaveSessionResultDto>.Created($"/api/session-results/{id}", dto);
    }

    public async Task<Result<CreateReadingComprehensionExerciseResponse>> CreateReadingComprehensionExerciseAsync(CreateReadingComprehensionExerciseRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<CreateReadingComprehensionExerciseResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var readingComprehension = await _aiLearningService.GenerateReadingComprehensionExerciseAsync(request);

        return Result<CreateReadingComprehensionExerciseResponse>.Ok(readingComprehension);
    }

    public async Task<Result<CheckReadingComprehensionExerciseResponse>> CheckReadingComprehensionExerciseAsync(CheckReadingComprehensionExerciseRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<CheckReadingComprehensionExerciseResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var response = await _aiLearningService.CheckReadingComprehensionExerciseAsync(request);

        return Result<CheckReadingComprehensionExerciseResponse>.Ok(response);
    }
}
