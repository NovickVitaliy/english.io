using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.Store.User;

namespace Learning.Features.Decks.Pages;

public partial class DeckPage : ComponentBase
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<DeckPage> Localizer { get; init; } = null!;
}

