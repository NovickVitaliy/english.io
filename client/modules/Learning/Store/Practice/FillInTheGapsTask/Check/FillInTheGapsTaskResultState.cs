using Fluxor;
using Learning.Features.Practice.Models.FillInTheGaps;

namespace Learning.Store.Practice.FillInTheGapsTask.Check;

[FeatureState]
public record FillInTheGapsTaskResultState(SentenceWithFilledGapResult[] SentencesWithFilledGapsResults, bool IsLoading)
{
    private FillInTheGapsTaskResultState() : this([], false)
    {

    }

    public SentenceWithFilledGapResult? ForWord(Guid senseId)
    {
        return SentencesWithFilledGapsResults.SingleOrDefault(x => x.SenseId == senseId);
    }
}
