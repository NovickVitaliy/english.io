using System.Text.Json;
using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ContrastTask;
using Learning.Application.DTOs.Practice.ExampleText;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.GetTranslationTask;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.Sessions;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;
using Learning.Infrastructure.DTOs;
using Microsoft.Extensions.Logging;
using Shared;
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
    private readonly ILogger<PracticeService> _logger;

    public PracticeService(
        IAiLearningService aiLearningService,
        IPracticeRepository practiceRepository,
        ICurrentUserAccessor currentUserAccessor,
        IDecksRepository decksRepository,
        IWordUnitService wordUnitService,
        ILogger<PracticeService> logger)
    {
        _aiLearningService = aiLearningService;
        _practiceRepository = practiceRepository;
        _currentUserAccessor = currentUserAccessor;
        _decksRepository = decksRepository;
        _wordUnitService = wordUnitService;
        _logger = logger;
    }

    public async Task<Result<TranslateWordsResponse>> CheckTranslateWordsTaskAsync(CheckTranslateWordsTaskRequest taskRequest)
    {
        var validationResult = taskRequest.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<TranslateWordsResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var response = await _aiLearningService.VerifyWordsTranslations(taskRequest);

        if (response is null)
        {
            return Result<TranslateWordsResponse>.BadRequest("Something went wrong");
        }

        var joined = response.Join(taskRequest.TranslatedWords,
            a => a.SenseId,
            b => b.SenseId,
            (a, b) => new
            {
                a, b
            });

        var tasks = joined.Select(async obj =>
        {
            var wordSenseFullInfo = await _wordUnitService.GetWordSenseFullInfo(obj.b.SenseId);

            return obj.a with
            {
                CorrectTranslation = taskRequest.OriginalLanguage == GlobalConstants.Languages.English ? wordSenseFullInfo!.UkrainianTranslation : wordSenseFullInfo!.EnglishWord,
                SenseId = wordSenseFullInfo.SenseId
            };
        });

        response = await Task.WhenAll(tasks);

        await UpdateWordsProgress(
            taskRequest.DeckId,
            response.Select(x => new WordSenseTaskResult(x.SenseId, x.IsCorrect)).ToArray(),
            taskRequest.OriginalLanguage == GlobalConstants.Languages.English ? PracticeTask.TranslateFromEnglishToUkrainian : PracticeTask.TranslateFromUkrainianToEnglish);

        return Result<TranslateWordsResponse>.Ok(new TranslateWordsResponse(response));
    }

    public async Task<Result<SentenceWithGap[]>> GetSentencesWithGapsAsync(Guid deckId, WordForPractice[] wordsForPractice)
    {
        var valid = wordsForPractice.Length > 0 && !wordsForPractice.All(w => string.IsNullOrWhiteSpace(w.Word));
        if (!valid)
        {
            return Result<SentenceWithGap[]>.BadRequest(WordsMustBePresent);
        }

        var sentencesWithGaps = await _aiLearningService.GenerateSentencesWithGaps(wordsForPractice);

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

    public async Task<Result<CreateReadingComprehensionExerciseResponse>> CreateReadingComprehensionExerciseAsync(WordForPractice[] wordsForPractice)
    {
        if (wordsForPractice.Length == 0)
        {
            return Result<CreateReadingComprehensionExerciseResponse>.BadRequest("Invalid reqeust");
        }

        var readingComprehension = await _aiLearningService.GenerateReadingComprehensionExerciseAsync(wordsForPractice);
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
            results.Select(x => new SessionDto(x.Words, x.FirstTaskPercentageSuccess, x.SecondTaskPercentageSuccess, x.ThirdTaskPercentageSuccess, x.FourthTaskPercentageSuccess,
                x.PracticeDate)).ToArray(),
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

    public async Task<Result<IReadOnlyCollection<WordForTranslationPractice>>> GetWordForTranslationTaskAsync(Guid deckId, GetTranlationTaskRequest request)
    {
        if (!GlobalConstants.Languages.SupportedLanguages.Contains(request.OriginalLanguage))
        {
            return Result<IReadOnlyCollection<WordForTranslationPractice>>.BadRequest(OriginalLanguageDoesNotExist);
        }

        if (!GlobalConstants.Languages.SupportedLanguages.Contains(request.TranslateLanguage))
        {
            return Result<IReadOnlyCollection<WordForTranslationPractice>>.BadRequest(TranslateLanguageDoesNotExist);
        }

        var deck = await _decksRepository.GetDeckAsync(deckId);
        if (deck is null)
        {
            return Result<IReadOnlyCollection<WordForTranslationPractice>>.NotFound(deckId);
        }

        List<WordForTranslationPractice> words = [];

        foreach (var wordForPractice in request.WordsForPractice)
        {
            var wordSenseFullInfo = await _wordUnitService.GetWordSenseFullInfo(wordForPractice.SenseId);

            if (wordSenseFullInfo is not null)
            {
                words.Add(request.OriginalLanguage == GlobalConstants.Languages.English
                    ? new WordForTranslationPractice(wordSenseFullInfo.EnglishWord, wordSenseFullInfo.EnglishDefinition, wordSenseFullInfo.SenseId)
                    : new WordForTranslationPractice(wordSenseFullInfo.UkrainianTranslation, "", wordSenseFullInfo.SenseId));
            }
        }

        return Result<IReadOnlyCollection<WordForTranslationPractice>>.Ok(words);
    }

    public async Task<Result<SentenceWithFilledGapResult[]>> CheckSentencesWithGapsTask(CheckSentencesWithGapsTaskRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<SentenceWithFilledGapResult[]>.BadRequest(validationResult.ErrorMessage);
        }

        var response = await _aiLearningService.CheckSentencesWithGapsTaskAsync(request.SentencesWithFilledGaps);
        if (response is null)
        {
            return Result<SentenceWithFilledGapResult[]>.BadRequest("Could not get response from the API");
        }
        await UpdateWordsProgress(
            request.DeckId,
            response.Select(x => new WordSenseTaskResult(x.SenseId, x.IsCorrect)).ToArray(),
            PracticeTask.FillInTheGaps);

        return Result<SentenceWithFilledGapResult[]>.Ok(response);
    }

    public async Task<Result<ContrastTaskUnit[]>> GetContrastTaskAsync(Guid deckId, WordForPractice[] wordsForPractice)
    {
        var valid = wordsForPractice.Length > 0 && !wordsForPractice.All(w => string.IsNullOrWhiteSpace(w.Word));
        if (!valid)
        {
            return Result<ContrastTaskUnit[]>.BadRequest(WordsMustBePresent);
        }

        var contrastTask = await _aiLearningService.GenerateContrastTaskForWordsAsync(wordsForPractice);
        if (contrastTask is null)
        {
            return Result<ContrastTaskUnit[]>.BadRequest("Error while generating response");
        }

        foreach (var contrastTaskUnit in contrastTask)
        {
            var wordForPractice = wordsForPractice.SingleOrDefault(x => x.Word == contrastTaskUnit.CorrectWord);
            if (wordForPractice is not null && wordForPractice.SenseId != contrastTaskUnit.SenseId)
            {
                contrastTaskUnit.SenseId = wordForPractice.SenseId;
            }
        }

        _logger.LogInformation("API Response from GEMINI API: {JsonResponse}", JsonSerializer.Serialize(contrastTask));

        foreach (var contrastTaskUnit in contrastTask)
        {
            contrastTaskUnit.PossibleChoices = await _wordUnitService.GetSynonymsForSenseAsync(contrastTaskUnit.SenseId);
        }

        return Result<ContrastTaskUnit[]>.Ok(contrastTask);
    }

    public async Task<Result<bool>> SaveContrastTaskResultAsync(SaveContrastTaskResultRequest request)
    {
        var deck = await _decksRepository.GetDeckAsync(request.DeckId);
        if (deck is null)
        {
            return Result<bool>.NotFound(request.DeckId);
        }

        await UpdateWordsProgress(
            request.DeckId,
            [.. request.AnswersMap.Select(a => new WordSenseTaskResult(a.Key, a.Value.IsCorrect))],
            PracticeTask.ContrastTask);

        return Result<bool>.NoContent();
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

    private async Task UpdateWordsProgress(Guid deckId, WordSenseTaskResult[] wordSenseTaskResults, PracticeTask practiceTask)
    {
        var success = Enum.TryParse<PracticeDifficulty>(_currentUserAccessor.GetPracticeDifficulty(), out var practiceDifficulty);
        if (!success)
        {
            practiceDifficulty = PracticeDifficulty.Medium;
        }

        var taskWeight = Constants.GetTaskWeight(practiceTask);
        var difficultyMultiplier = Constants.GetDifficultyMultiplier(practiceDifficulty);
        var forgettingFactor = Constants.GetForgettingFactor(practiceDifficulty);
        var now = DateTimeOffset.UtcNow;
        var deck = await _decksRepository.GetDeckAsync(deckId);

        if (deck is null) return;

        foreach (var result in wordSenseTaskResults)
        {
            var deckEntry = deck!.DeckEntries.SingleOrDefault(x => x.WordSenseId == result.SenseId);

            if (deckEntry is not null)
            {
                var taskEffect = Constants.BaseLearningRate * taskWeight * difficultyMultiplier;

                if (result.IsCorrect)
                {
                    deckEntry.ProgressScore += (1 - deckEntry.ProgressScore) * taskEffect;
                }
                else
                {
                    deckEntry.ProgressScore -= deckEntry.ProgressScore * taskEffect;
                }

                float daysSinceLastPractice = 0f;
                if (deckEntry.LastTimePracticed is not null)
                {
                    daysSinceLastPractice = (now - deckEntry.LastTimePracticed).Value.Days;
                }

                deckEntry.ProgressScore *= (float)Math.Pow(forgettingFactor, daysSinceLastPractice);

                deckEntry.ProgressScore = (float)Math.Round(Math.Clamp(deckEntry.ProgressScore, 0f, 1f), 2);
                deckEntry.LastTimePracticed = now;
            }
        }

        await _decksRepository.UpdateDeckAsync(deckId, deck);
    }
}
