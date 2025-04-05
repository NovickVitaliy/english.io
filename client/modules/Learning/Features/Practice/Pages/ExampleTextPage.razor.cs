using System.Text.RegularExpressions;
using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.Store.User;

namespace Learning.Features.Practice.Pages;

public partial class ExampleTextPage : ComponentBase
{
    [Inject] private IStringLocalizer<ExampleTextPage> Localizer { get; init; } = null!;
    [Inject] private IState<FIllInTheGapsState> FillInTheGapsState { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    private string? _exampleText;
    private bool _finishPracticeButtonDisabled = true;
    private const string UsedWordPattern = @"\*(.*?)\*";
    private const string Replacement = "<b>$1</b>";

    protected override async Task OnParametersSetAsync()
    {
        if (UserState.Value is not null && FillInTheGapsState.Value is not null)
        {
            var response = await PracticeService.GenerateExampleTextAsync(FillInTheGapsState.Value.Words, UserState.Value.Token);
            _exampleText = Regex.Replace(response.Text, UsedWordPattern, Replacement);
            _ = Task.Factory.StartNew(() =>
            {
                Task.Delay(TimeSpan.FromSeconds(5));
                _finishPracticeButtonDisabled = false;
            });
        }
    }

    private Task FinishPractice()
    {
        // call api to finish practice with all the data, etc.
        throw new NotImplementedException();
    }
}

