using System.Text.RegularExpressions;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Components;
using Learning.Features.Practice.Models;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.Practice.ExampleText.Get;
using Learning.Store.Practice.ExampleText.Get.Actions;
using Learning.Store.PracticeStatus;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared.Store.User;

namespace Learning.Features.Practice.Pages;

public partial class ExampleTextPage : FluxorComponent
{
    [Inject] private IStringLocalizer<ExampleTextPage> Localizer { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private IState<ExampleTextTaskState> ExampleTextTaskState { get; init; } = null!;
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    private const string UsedWordPattern = @"\*(.*?)\*";
    private const string Replacement = "<b>$1</b>";

    protected override async Task OnParametersSetAsync()
    {
        Dispatcher.Dispatch(new GetExampleTextTaskAction(PracticeState.Value.WordsForPractice));
    }

    private Task FinishPractice()
    {
        NavigationManager.NavigateTo("123");
        return Task.CompletedTask;
    }
}
