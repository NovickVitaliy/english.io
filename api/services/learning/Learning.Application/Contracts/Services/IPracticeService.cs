using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ExampleText;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.TranslateWords;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IPracticeService
{
    Task<Result<TranslateWordsResponse>> TranslateWords(TranslateWordsRequest request);
    Task<Result<SentenceWithGap[]>> GetSentencesWithGapsAsync(string[] words);
    Task<Result<GetExampleTextResponse>> GetExampleTextAsync(string[] words);
}
