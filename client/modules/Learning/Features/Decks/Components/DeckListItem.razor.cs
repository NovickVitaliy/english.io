using System.ComponentModel.DataAnnotations;
using Learning.Features.Decks.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Features.Decks.Components;

public partial class DeckListItem : ComponentBase
{
    [Parameter, EditorRequired] public DeckDto DeckDto { get; init; } = null!;
}

