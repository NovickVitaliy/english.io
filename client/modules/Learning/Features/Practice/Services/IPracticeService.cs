using Learning.Features.Practice.Models;
using Learning.Features.Practice.Models.ExampleText;
using Learning.Features.Practice.Models.FillInTheGaps;
using Learning.Features.Practice.Models.ReadingComprehension;
using Learning.Features.Settings.Models.Sessions;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Learning.Features.Practice.Services;

public interface IPracticeService
{
    const string ApiUrlKey = "Learning";

    [Post("/practice/translate-words")]
    Task<TranslateWordsResponse> TranslateWords(TranslateWordsRequest request, [Authorize] string token);

    [Get("/practice/sentences-with-gaps")]
    Task<SentenceWithGap[]> GenerateSentencesWithGaps([Query(CollectionFormat.Multi)] string[] words, [Authorize] string token);

    [Get("/practice/example-text")]
    Task<GenerateExampleTextResponse> GenerateExampleTextAsync([Query(CollectionFormat.Multi)] string[] words, [Authorize] string token);

    [Post("/practice/save-session-result")]
    Task<SaveSessionResultDto> SaveSessionResult(SaveSessionResultRequest request);

    [Get("/practice/reading-comprehension")]
    Task<ReadingComprehensionExercise> GetReadingComprehensionExercise([Query(CollectionFormat.Multi)]string[] words, [Authorize] string token);

    [Post("/practice/reading-comprehension-check")]
    Task<CheckReadingComprehensionExerciseResult> CheckReadingComprehensionExercise(CheckReadingComprehensionExerciseRequest request, [Authorize] string token);

    [Get("/practice/sessions")]
    Task<GetSessionsResultsForUserResponse> FetchSessionForUserAsync([Query] GetSessionResultsForUserRequest getSessionResultsForUserRequest, [Authorize] string token);
}
