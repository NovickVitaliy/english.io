using Fluxor;
using Learning.Features.Practice.Models.FillInTheGaps;

namespace Learning.Store.Practice.FillInTheGapsTask.Get;

[FeatureState]
public record FillInTheGapsTaskState(SentenceWithGap[] SentencesWithGap, bool IsLoading)
{
    private FillInTheGapsTaskState() : this([], false)
    {

    }
}
