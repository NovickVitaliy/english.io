using Learning.Features.Practice.Models;
using Learning.Features.Practice.Models.ContrastTask;
using Learning.Features.Practice.Models.ExampleText;
using Learning.Features.Practice.Models.FillInTheGaps;
using Learning.Features.Practice.Models.GetWordsForPractice;
using Learning.Features.Practice.Models.ReadingComprehension;
using Learning.Features.Practice.Models.TranslateWordsTask.Check;
using Learning.Features.Practice.Models.TranslateWordsTask.Get;
using Learning.Features.Settings.Models.Sessions;
using Refit;

namespace Learning.Features.Practice.Services;

public interface IPracticeService
{
    const string ApiUrlKey = "Learning";

    [Post("/practice/check-translation-task")]
    Task<CheckTranslateWordsTaskResponse> CheckTranslateWordsTaskAsync(CheckTranslateWordsTaskRequest taskRequest, [Authorize] string token);

    [Post("/practice/{deckId}/get-sentences-with-gaps-task")]
    Task<SentenceWithGap[]> GetFillInTheGapsTaskAsync(Guid deckId, WordForPractice[] wordsForPractice, [Authorize] string token);

    [Post("/practice/check-sentences-with-gaps-task")]
    Task<SentenceWithFilledGapResult[]> CheckFillInTheGapsTaskAsync(CheckFillInTheGapsTaskRequest request, [Authorize] string token);

    [Get("/practice/example-text")]
    Task<GenerateExampleTextResponse> GenerateExampleTextAsync([Query(CollectionFormat.Multi)] string[] words, [Authorize] string token);

    [Post("/practice/save-session-result")]
    Task<SaveSessionResultDto> SaveSessionResult(SaveSessionResultRequest request, [Authorize] string token);

    [Post("/practice/get-reading-comprehension")]
    Task<ReadingComprehensionExercise> GetReadingComprehensionExercise(WordForPractice[] wordForPractices, [Authorize] string token);

    [Post("/practice/reading-comprehension-check")]
    Task<CheckReadingComprehensionExerciseResult> CheckReadingComprehensionExercise(CheckReadingComprehensionExerciseRequest request, [Authorize] string token);

    [Get("/practice/sessions")]
    Task<GetSessionsResultsForUserResponse> FetchSessionForUserAsync([Query] GetSessionResultsForUserRequest getSessionResultsForUserRequest, [Authorize] string token);

    [Get("/practice/{deckId}/words")]
    Task<GetWordsForPracticeResponse> GetWordsForPracticeAsync([Query] Guid deckId, [Authorize] string token);

    [Post("/practice/{deckId}/get-translation-task")]
    Task<WordForTranslationPractice[]> GetTranslationTaskAsync([Query] Guid deckId, GetTranlationTaskRequest request, [Authorize] string token);

    [Post("/practice/{deckId}/get-contrast-task")]
    Task<ContrastTaskUnit[]> GetContrastTaskAsync([Query] Guid deckId, WordForPractice[] wordsForPractice, [Authorize] string token);

    [Post("/practice/save-contrast-task")]
    Task SaveContrastTaskResultAsync(SaveContrastTaskResultRequest request, [Authorize] string token);
}
