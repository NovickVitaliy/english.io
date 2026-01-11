namespace Learning.Features.Practice.Models.TranslateWordsTask.Check;

public record TranslateWordResult(string OriginalWord, string TranslatedWord, bool IsCorrect, string CorrectTranslation, Guid SenseId);
