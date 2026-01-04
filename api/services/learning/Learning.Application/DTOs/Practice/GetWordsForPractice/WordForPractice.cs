namespace Learning.Application.DTOs.Practice.GetWordsForPractice;

public record WordForPractice(
    string PartOfSpeech,
    Guid WordId,
    string Word,
    Guid SenseId,
    string Sense);
