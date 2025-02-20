using FluentValidation;
using Learning.Application.DTOs.Decks;

namespace Learning.Application.Validation.Decks;

public class CreateDeckWordRequestValidator : AbstractValidator<CreateDeckWordRequest>
{
    public CreateDeckWordRequestValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty().WithMessage("Word cannot be empty");

        RuleFor(x => x.DeckId)
            .NotEmpty().WithMessage("Deck Id cannot be empty");
    }
}
