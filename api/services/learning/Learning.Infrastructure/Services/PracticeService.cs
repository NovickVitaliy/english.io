using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ExampleText;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.Sessions;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using Shared.ErrorHandling;
using Shared.Services.Contracts;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Infrastructure.Services;

public class PracticeService : IPracticeService
{
    private readonly IAiLearningService _aiLearningService;
    private readonly IPracticeRepository _practiceRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDecksRepository _decksRepository;
    private readonly IWordUnitService _wordUnitService;

    public PracticeService(
        IAiLearningService aiLearningService,
        IPracticeRepository practiceRepository,
        ICurrentUserAccessor currentUserAccessor,
        IDecksRepository decksRepository,
        IWordUnitService wordUnitService)
    {
        _aiLearningService = aiLearningService;
        _practiceRepository = practiceRepository;
        _currentUserAccessor = currentUserAccessor;
        _decksRepository = decksRepository;
        _wordUnitService = wordUnitService;
    }

    public async Task<Result<TranslateWordsResponse>> TranslateWords(TranslateWordsRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<TranslateWordsResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var response = await _aiLearningService.VerifyWordsTranslations(request);

        if (response is null)
        {
            return Result<TranslateWordsResponse>.BadRequest("Something went wrong");
        }

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

        if (sentencesWithGaps is null)
        {
            return Result<SentenceWithGap[]>.BadRequest("Something went wrong");
        }

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
        if (readingComprehension is null)
        {
            return Result<CreateReadingComprehensionExerciseResponse>.BadRequest("Something went wrong");
        }

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
        if (response is null)
        {
            return Result<CheckReadingComprehensionExerciseResponse>.BadRequest("Something went wrong");
        }

        return Result<CheckReadingComprehensionExerciseResponse>.Ok(response);
    }
    public async Task<Result<GetPracticeSessionsForUserResponse>> GetSessionsForUserAsync(GetSessionsForUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserEmail))
        {
            return Result<GetPracticeSessionsForUserResponse>.BadRequest(UserEmailCannotBeEmpty);
        }

        var (results, count) = await _practiceRepository.GetSessionsResultsForUserAsync(request);

        return Result<GetPracticeSessionsForUserResponse>.Ok(new GetPracticeSessionsForUserResponse(
                results.Select(x => new SessionDto(x.Words, x.FirstTaskPercentageSuccess, x.SecondTaskPercentageSuccess, x.ThirdTaskPercentageSuccess, x.FourthTaskPercentageSuccess, x.PracticeDate)).ToArray(),
                count));
    }

    public async Task<Result<GetWordsForPracticeResponse>> GetWordsForPracticeAsync(Guid deckId)
    {
        var deck = await _decksRepository.GetDeckAsync(deckId);
        if (deck is null)
        {
            return Result<GetWordsForPracticeResponse>.NotFound(deckId);
        }

        var practiceBatch = SelectPracticeBatch(
            deck.DeckEntries,
            _currentUserAccessor.GetCountOfWordsForPractice(),
            _currentUserAccessor.GetPracticeDifficulty());

        var wordsForPractice = await _wordUnitService.GetPracticeWords(practiceBatch);

        return Result<GetWordsForPracticeResponse>.Ok(new GetWordsForPracticeResponse(wordsForPractice));
    }

    private static List<DeckEntry> SelectPracticeBatch(List<DeckEntry> deckEntries, int? countOfWordsForPractice, string? practiceDifficulty)
    {
        var weightedDeck = new List<(DeckEntry DeckEntry, float Weight)>();
        var now = DateTimeOffset.UtcNow;

        foreach (var de in deckEntries)
        {
            float totalDays = 0;
            if (de.LastTimePracticed is not null)
            {
                totalDays = (float)(now - de.LastTimePracticed).Value.TotalDays;
            }

            var forgettingFactor = Constants.GetForgettingFactor(Enum.Parse<PracticeDifficulty>(practiceDifficulty!));
            float adjustedProgress = de.ProgressScore * (float)Math.Pow(forgettingFactor, totalDays);

            float weight = (1 - adjustedProgress);

            weightedDeck.Add((de, weight));
        }

        var session = new List<DeckEntry>();

        float totalWeight = weightedDeck.Sum(x => x.Weight);

        while (session.Count < countOfWordsForPractice && weightedDeck.Count > 0)
        {
            float r = (float)(Random.Shared.NextDouble() * totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < weightedDeck.Count; i++)
            {
                cumulative += weightedDeck[i].Weight;
                if (r <= cumulative)
                {
                    var selected = weightedDeck[i].DeckEntry;
                    session.Add(selected);

                    totalWeight -= weightedDeck[i].Weight;
                    weightedDeck.RemoveAt(i);
                    break;
                }
            }
        }

        return session;
    }
}
