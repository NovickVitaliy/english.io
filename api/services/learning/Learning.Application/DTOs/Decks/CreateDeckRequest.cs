using Learning.Application.Validation.Decks;
using Shared.Requests;

namespace Learning.Application.DTOs.Decks;

public record CreateDeckRequest(string DeckTopic, bool IsStrict) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new CreateDeckRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors[0].ErrorMessage);
    }
}
