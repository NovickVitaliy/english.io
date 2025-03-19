using FluentValidation;
using Learning.Application.DTOs.Decks;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.Validation.Decks;

public class CreateDeckWordRequestValidator : AbstractValidator<CreateDeckWordRequest>
{
    public CreateDeckWordRequestValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty().WithMessage(WordCannotBeEmpty);

        RuleFor(x => x.DeckId)
            .NotEmpty().WithMessage(DeckIdCannotBeEmpty);
    }
}
