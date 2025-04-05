namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslatedWord(
    string OriginalWord,
    string Translated,
    string OriginalLanguage,
    string TranslatedLanguage);
