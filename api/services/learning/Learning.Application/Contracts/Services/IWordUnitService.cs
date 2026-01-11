using Learning.Application.DTOs.Practice.GetTranslationTask;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Services;

public interface IWordUnitService
{
    Task<WordUnit> GetOrCreateWordUnit(string word);
    Task<Guid?> GetWordUnitIdByWord(string word);
    Task<IReadOnlyCollection<WordUnit>> GetWordUnitsFromUsersDeck(Deck deck);
    Task<WordForPractice[]> GetPracticeWords(List<DeckEntry> practiceWordFilters);
    Task<WordSenseFullInfo?> GetWordSenseFullInfo(Guid senseId);
}
