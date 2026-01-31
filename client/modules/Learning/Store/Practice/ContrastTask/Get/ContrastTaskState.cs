using Fluxor;
using Learning.Features.Practice.Models.ContrastTask;

namespace Learning.Store.Practice.ContrastTask.Get;

[FeatureState]
public record ContrastTaskState(ContrastTaskUnit[] ContrastTaskUnits, bool IsLoading, bool IsSaving = false)
{
    private ContrastTaskState() : this([], false)
    {

    }

    public string? ForSense(Guid senseId)
    {
        return ContrastTaskUnits.SingleOrDefault(x => x.SenseId == senseId)?.CorrectWord;
    }
}
