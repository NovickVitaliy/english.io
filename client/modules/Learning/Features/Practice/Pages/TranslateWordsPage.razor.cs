using System.Text.Json;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Models;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.Practice.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Practice.Pages;

public partial class TranslateWordsPage : FluxorComponent
{
    [Inject] private IStringLocalizer<TranslateWordsPage> Localizer { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [SupplyParameterFromQuery] private string OriginalLanguage { get; init; } = null!;
    [SupplyParameterFromQuery] private string TranslateLanguage { get; init; } = null!;
    [SupplyParameterFromQuery] private int WordsCount { get; init; }
    private TranslateWordsRequest _request = null!;
    private TranslateWordsResponse? _response = null!;
    private bool _overlayVisible = false;

    protected override void OnParametersSet()
    {
        if (!(GlobalConstants.Languages.SupportedLanguages.Contains(OriginalLanguage)
            && GlobalConstants.Languages.SupportedLanguages.Contains(TranslateLanguage)))
        {
            NavigationManager.NavigateTo("/learning/decks");
            return;
        }

        _request = new TranslateWordsRequest(WordsCount);
    }

    private async Task VerifyTranslatedWords()
    {
        _overlayVisible = true;
        if (_response != null)
        {
            Snackbar.Add(Localizer["Already_Verified"], Severity.Info);
        }

        try
        {
            _response = await PracticeService.TranslateWords(_request, UserState.Value.Token);
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(problemDetails.Detail ?? "Error_Occured", Severity.Error);
        }
        finally
        {
            _overlayVisible = false;
        }
    }

    private void NextExercise()
    {
        if (OriginalLanguage == ClientConstants.Languages.Ukrainian)
        {
            Dispatcher.Dispatch(new SetWordsBeingPracticedAction(_response?.Results.Select(x => x.CorrectTranslation).ToArray() ?? []));
            NavigationManager.NavigateTo("/practice/fill-in-the-gaps");
            return;
        }
        Dispatcher.Dispatch(new SetWordsBeingPracticedAction(_response?.Results.Select(x => x.CorrectTranslation).ToArray() ?? []));
        _response = null;
        NavigationManager.NavigateTo($"/practice/translate-words?originalLanguage=ukrainian&translateLanguage=english&wordsCount={WordsCount}");
    }
}

