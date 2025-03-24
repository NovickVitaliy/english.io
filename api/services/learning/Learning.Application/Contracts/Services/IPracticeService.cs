using Learning.Application.DTOs.Practice;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IPracticeService
{
    Task<Result<TranslateWordsResponse>> TranslateWords(TranslateWordsRequest request);
}
