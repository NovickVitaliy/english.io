using Learning.Features.Practice.Models.GetWordsForPractice;

namespace Learning.Features.Practice.Models.TranslateWordsTask.Get;

public record GetTranlationTaskRequest(string OriginalLanguage, string TranslateLanguage, WordForPractice[] WordsForPractice);
