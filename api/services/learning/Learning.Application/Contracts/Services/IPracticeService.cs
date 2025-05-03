using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ExampleText;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.Sessions;
using Learning.Application.DTOs.Practice.TranslateWords;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IPracticeService
{
    Task<Result<TranslateWordsResponse>> TranslateWords(TranslateWordsRequest request);
    Task<Result<SentenceWithGap[]>> GetSentencesWithGapsAsync(string[] words);
    Task<Result<GetExampleTextResponse>> GetExampleTextAsync(string[] words);
    Task<Result<SaveSessionResultDto>> SaveSessionResultAsync(SaveSessionResultRequest request);
    Task<Result<CreateReadingComprehensionExerciseResponse>> CreateReadingComprehensionExerciseAsync(CreateReadingComprehensionExerciseRequest request);
    Task<Result<CheckReadingComprehensionExerciseResponse>> CheckReadingComprehensionExerciseAsync(CheckReadingComprehensionExerciseRequest request);
    Task<Result<GetPracticeSessionsForUserResponse>> GetSessionsForUserAsync(GetSessionsForUserRequest request);
}
