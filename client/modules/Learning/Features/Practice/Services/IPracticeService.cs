using Learning.Features.Practice.Models;
using Learning.Features.Practice.Models.FillInTheGaps;
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
}
