using Learning.Features.Practice.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Learning.Features.Practice.Services;

public interface IPracticeService
{
    const string ApiUrlKey = "Learning";

    [Post("/practice/translate-words")]
    Task<TranslateWordsResponse> TranslateWords(TranslateWordsRequest request, [Authorize] string token);
}
