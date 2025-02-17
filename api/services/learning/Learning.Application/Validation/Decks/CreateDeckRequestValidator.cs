using FluentValidation;
using Learning.Application.DTOs.Decks;

namespace Learning.Application.Validation.Decks;

public class CreateDeckRequestValidator : AbstractValidator<CreateDeckRequest>
{
    public CreateDeckRequestValidator()
    {
        RuleFor(x => x.DeckTopic)
            .NotEmpty().WithMessage("Deck Topic cannot be empty");
    }
}
