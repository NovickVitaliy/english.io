using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class DecksToolbar : ComponentBase
{
    private string _searchQuery = "";

    [Inject]
    private IDialogService DialogService { get; init; } = null!;

    private Task<IDialogReference> ShowCreateDeckDialog()
    {
        var options = new DialogOptions()
        {
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        return DialogService.ShowAsync<CreateDeckDialog>(Localizer["Dialog_Name"], options);
    }
}

