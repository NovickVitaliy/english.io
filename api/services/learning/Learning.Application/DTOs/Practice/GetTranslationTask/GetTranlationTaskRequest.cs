using Learning.Application.DTOs.Practice.GetWordsForPractice;

namespace Learning.Application.DTOs.Practice.GetTranslationTask;

public record GetTranlationTaskRequest(
    string OriginalLanguage,
    string TranslateLanguage,
    WordForPractice[] WordsForPractice);
