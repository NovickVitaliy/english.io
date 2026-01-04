namespace Learning.Features.Practice.Models.GetWordsForPractice;

public record WordForPractice(
    string PartOfSpeech,
    Guid WordId,
    string Word,
    Guid SenseId,
    string Sense);
