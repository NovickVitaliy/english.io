namespace Learning.Application.DTOs.Practice.GetTranslationTask;

public record WordSenseFullInfo(
    Guid SenseId,
    string EnglishWord,
    string EnglishDefinition,
    string UkrainianTranslation);
