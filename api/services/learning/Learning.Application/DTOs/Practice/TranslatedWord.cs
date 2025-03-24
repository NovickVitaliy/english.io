namespace Learning.Application.DTOs.Practice;

public record TranslatedWord(
    string OriginalWord,
    string Translated,
    string OriginalLanguage,
    string TranslatedLanguage);
