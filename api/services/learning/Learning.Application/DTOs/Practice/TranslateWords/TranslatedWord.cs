namespace Learning.Application.DTOs.Practice.TranslateWords;

public record TranslatedWord(
    string OriginalWord,
    string Definition,
    string? Translated,
    Guid SenseId);
