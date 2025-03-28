namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslatedWordResult(string OriginalWord, string TranslatedWord, bool IsCorrect, bool CorrectTranslation);
