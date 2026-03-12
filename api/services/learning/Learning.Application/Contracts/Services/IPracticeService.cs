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
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IPracticeService
{
    Task<Result<TranslateWordsResponse>> CheckTranslateWordsTaskAsync(CheckTranslateWordsTaskRequest taskRequest);
    Task<Result<SentenceWithGap[]>> GetSentencesWithGapsAsync(Guid deckId, WordForPractice[] wordsForPractice);
    Task<Result<GetExampleTextResponse>> GetExampleTextAsync(string[] words);
    Task<Result<SaveSessionResultDto>> SaveSessionResultAsync(SaveSessionResultRequest request);
    Task<Result<CreateReadingComprehensionExerciseResponse>> CreateReadingComprehensionExerciseAsync(WordForPractice[] wordsForPractice);
    Task<Result<CheckReadingComprehensionExerciseResponse>> CheckReadingComprehensionExerciseAsync(CheckReadingComprehensionExerciseRequest request);
    Task<Result<GetPracticeSessionsForUserResponse>> GetSessionsForUserAsync(GetSessionsForUserRequest request);
    Task<Result<GetWordsForPracticeResponse>> GetWordsForPracticeAsync(Guid deckId);
    Task<Result<IReadOnlyCollection<WordForTranslationPractice>>> GetWordForTranslationTaskAsync(Guid deckId, GetTranlationTaskRequest request);
    Task<Result<SentenceWithFilledGapResult[]>> CheckSentencesWithGapsTask(CheckSentencesWithGapsTaskRequest request);
    Task<Result<ContrastTaskUnit[]>> GetContrastTaskAsync(Guid deckId, WordForPractice[] wordsForPractice);
    Task<Result<bool>> SaveContrastTaskResultAsync(SaveContrastTaskResultRequest request);
}
