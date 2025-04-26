using System.Text.RegularExpressions;
using Fluxor;
using Learning.Features.Practice.Components;
using Learning.Features.Practice.Models;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.PracticeStatus;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Practice.Pages;

public partial class ExampleTextPage : ComponentBase
{
    [Inject] private IStringLocalizer<ExampleTextPage> Localizer { get; init; } = null!;
    [Inject] private IState<FIllInTheGapsState> FillInTheGapsState { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    [Inject] private IState<PracticeStatusState> PracticeStatusState { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;
    private string? _exampleText;
    private const string UsedWordPattern = @"\*(.*?)\*";
    private const string Replacement = "<b>$1</b>";

    protected override async Task OnParametersSetAsync()
    {
        if (UserState.Value is not null && FillInTheGapsState.Value is not null)
        {
            var response = await PracticeService.GenerateExampleTextAsync(FillInTheGapsState.Value.Words, UserState.Value.Token);
            _exampleText = Regex.Replace(response.Text, UsedWordPattern, Replacement);
        }
    }

    private async Task<IDialogReference> FinishPractice()
    {
        try
        {
            var response = await PracticeService.SaveSessionResult(new SaveSessionResultRequest(
                FillInTheGapsState.Value.Words,
                PracticeStatusState.Value.FirstTaskPercentageSuccess,
                PracticeStatusState.Value.SecondTaskPercentageSuccess,
                PracticeStatusState.Value.ThirdTaskPercentageSuccess,
                PracticeStatusState.Value.FourthTaskPercentageSuccess));

            var options = new DialogOptions()
            {
                CloseButton = true, CloseOnEscapeKey = true
            };

            var parameters = new DialogParameters<SessionResultModal>
            {
                {
                    x => x.SaveSessionResultDto, response
                }
            };

            return await DialogService.ShowAsync<SessionResultModal>(Localizer["Dialog_Name"], parameters, options);
        }
        catch (ApiException)
        {
            Snackbar.Add(Localizer["Error_Occured"], Severity.Error);
            return null!;
        }
    }
}
