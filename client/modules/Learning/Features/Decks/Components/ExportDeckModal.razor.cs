using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Decks.Components;

public partial class ExportDeckModal : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IStringLocalizer<ExportDeckModal> Localizer { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    private ExportFileOptions _chosenOption = ExportFileOptions.Csv;
    [Parameter] public Guid DeckId { get; init; }

    private async Task ExportFile()
    {
        try
        {
            var response = await DecksService.ExportToFile(DeckId, _chosenOption, UserState.Value.Token);
            response.EnsureSuccessStatusCode();
            var contentDisposition = response.Content.Headers.ContentDisposition;
            var contentType = response.Content.Headers.ContentType;
            var fileName = contentDisposition?.FileName ?? "decks.csv";

            var stream = await response.Content.ReadAsStreamAsync();

            var streamReference = new DotNetStreamReference(stream);
            await JsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamReference, contentType);
            MudDialog.Close(DialogResult.Ok(this));
        }
        catch (Exception)
        {
            Snackbar.Add(Localizer["Error_Occured"], Severity.Error);
        }
    }
}
