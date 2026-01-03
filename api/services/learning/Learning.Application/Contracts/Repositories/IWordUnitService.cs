using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IWordUnitService
{
    Task<WordUnit> GetOrCreateWordUnit(string word);
    Task<Guid?> GetWordUnitIdByWord(string word);
    Task<IReadOnlyCollection<WordUnit>> GetWordUnitsFromUsersDeck(Deck deck);
}
