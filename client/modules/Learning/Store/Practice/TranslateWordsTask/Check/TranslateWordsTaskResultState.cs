using Fluxor;
using Learning.Features.Practice.Models.TranslateWordsTask.Check;

namespace Learning.Store.Practice.TranslateWordsTask.Check;

[FeatureState]
public record TranslateWordsTaskResultState(TranslateWordResult[] Results, bool IsLoading)
{
    private TranslateWordsTaskResultState() : this([], false)
    {

    }

    public TranslateWordResult? ForWord(Guid senseId)
    {
        return Results.SingleOrDefault(x => x.SenseId == senseId);
    }
}
