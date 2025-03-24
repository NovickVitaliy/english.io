using System.Security.Claims;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.LearningShared.Services;
using Learning.Store.Deck;
using Learning.Store.Deck.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared;
using Shared.Store.User;

namespace Learning.Features.Practice.Pages;

public partial class ChooseWordsForPracticePage : FluxorComponent
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IState<DeckState> DeckState { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IStringLocalizer<ChooseWordsForPracticePage> Localizer { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; init; } = null!;
    private readonly List<string> _wordsForPractice = [];
    private int _countOfWordsForPractice;
    protected override void OnInitialized()
    {
        UserState.StateChanged += (_, _) =>
        {
            Dispatcher.Dispatch(new FetchDeckAction(DeckId));
        };
        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        _countOfWordsForPractice = int.Parse((await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims
            .SingleOrDefault(x => x.Type == GlobalConstants.ApplicationClaimTypes.CountOfWordsForPractice)?.Value ?? "10");
    }

    private void CheckWordForPractice(bool added, string word)
    {
        if (!added && _wordsForPractice.Contains(word))
        {
            _wordsForPractice.Remove(word);
            return;
        }

        if (_wordsForPractice.Count >= _countOfWordsForPractice)
        {
            Snackbar.Add(Localizer["Already_10_Words", _countOfWordsForPractice], Severity.Info);
            return;
        }

        _wordsForPractice.Add(word);
        Console.WriteLine(string.Join(',', _wordsForPractice));
    }
}

