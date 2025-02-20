using Learning.Application.Validation.Decks;
using Shared.Requests;

namespace Learning.Application.DTOs.Decks;

public record CreateDeckWordRequest(Guid DeckId, string Word) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new CreateDeckWordRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors[0].ErrorMessage);
    }
}
