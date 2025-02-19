using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Shared.Store.User;

namespace Learning.Features.Decks.Pages;

public partial class DeckPage : ComponentBase
{
    [Parameter] public Guid DeckId { get; init; }
}

