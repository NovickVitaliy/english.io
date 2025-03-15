using FluentValidation;
using Learning.Application.DTOs.Decks;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.Validation.Decks;

public class CreateDeckRequestValidator : AbstractValidator<CreateDeckRequest>
{
    public CreateDeckRequestValidator()
    {
        RuleFor(x => x.DeckTopic)
            .NotEmpty().WithMessage(DeckTopicCannotBeEmpty);
    }
}
