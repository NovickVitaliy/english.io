using Fluxor;
using Learning.Features.Practice.Models.ReadingComprehension;

namespace Learning.Store.Practice.ReadingComprehensionTask.Check;

[FeatureState]
public record ReadingComprehesionTaskResultState(CheckReadingComprehensionExerciseResult? Result, bool IsLoading)
{
    private ReadingComprehesionTaskResultState() : this(null, false)
    {

    }
}
