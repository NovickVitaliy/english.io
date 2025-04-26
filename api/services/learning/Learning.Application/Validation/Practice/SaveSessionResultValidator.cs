using FluentValidation;
using Learning.Application.DTOs.Practice;

namespace Learning.Application.Validation.Practice;

public class SaveSessionResultValidator : AbstractValidator<SaveSessionResultRequest>
{
    public SaveSessionResultValidator()
    {
        RuleFor(x => x.Words)
            .NotEmpty();

        RuleFor(x => x.FirstTaskPercentageSuccess)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.SecondTaskPercentageSuccess)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.ThirdTaskPercentageSuccess)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.FourthTaskPercentageSuccess)
            .InclusiveBetween(0, 100);
    }
}
