using Learning.Features.Decks.Components;
using Shared.LocalizedDataAnnotations;

namespace Learning.Features.Decks.Models;

public class CreateDeckWordRequest
{
    [LocalizedRequired(Constants.Localization.AddDeckWordFormBaseName, Constants.ValidationErrors.AddDeckWord.Required, typeof(AddDeckWordModal))]
    public string Word { get; set; } = null!;
}
