using Learning.Features.Practice.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.Practice.Components;

public partial class SessionResultModal : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Parameter] public SaveSessionResultDto SaveSessionResultDto { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(this));
        NavigationManager.NavigateTo("/learning/decks");
    }

    private static Color GetColorBasedOnSuccessPercentage(double firstTaskPercentageSuccess)
    {
        return firstTaskPercentageSuccess switch
        {
            >= 75 => Color.Success,
            >= 50 and < 75 => Color.Warning,
            < 50 => Color.Error,
            _ => Color.Default
        };
    }
}

