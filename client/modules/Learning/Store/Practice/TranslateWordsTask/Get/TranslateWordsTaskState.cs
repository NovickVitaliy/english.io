using Fluxor;
using Learning.Features.Practice.Models.TranslateWordsTask.Get;

namespace Learning.Store.Practice.TranslateWordsTask.Get;

[FeatureState]
public record TranslateWordsTaskState(WordForTranslationPractice[] Words)
{
    private TranslateWordsTaskState() : this([])
    {

    }
}
