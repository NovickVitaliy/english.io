using Fluxor;
using Learning.Features.Practice.Models.ReadingComprehension;

namespace Learning.Store.Practice.ReadingComprehensionTask.Get;

[FeatureState]
public record ReadingComprehensionTaskState(ReadingComprehensionExercise? ReadingComprehensionExercise, bool IsLoading)
{
    private ReadingComprehensionTaskState() : this(null, false)
    {

    }
}
