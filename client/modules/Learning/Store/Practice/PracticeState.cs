using Fluxor;
using Learning.Features.Practice.Models.GetWordsForPractice;

namespace Learning.Store.Practice;

[FeatureState]
public record PracticeState(WordForPractice[] WordsForPractice)
{
    private PracticeState() : this([])
    {

    }
}
