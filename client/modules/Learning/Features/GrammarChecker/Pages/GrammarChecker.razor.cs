using System.Globalization;
using Fluxor;
using Learning.Features.GrammarChecker.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.GrammarChecker.Pages;

public partial class GrammarChecker : ComponentBase
{
    [Inject] private GrammarCheckerService GrammarCheckerService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    private bool _hasAnalyzed;

    private string _text = string.Empty;

    private bool _loading;

    private List<TextIssueVm> _issues = [];

    private async Task AnalyzeText()
    {
        _loading = true;
        _issues.Clear();
        _hasAnalyzed = false;

        try
        {
            var result = await GrammarCheckerService
                .AnalyzeTextAsync(_text, UserState.Value.Token);

            _issues = result?.Issues
                .Select(x => new TextIssueVm(
                    x.Id,
                    x.Original,
                    x.Suggested,
                    x.Explanation,
                    x.Category,
                    x.StartIndex,
                    x.Length))
                .ToList() ?? [];

            _hasAnalyzed = true;
        }
        finally
        {
            _loading = false;
        }
    }

    private void ApplyFix(TextIssueVm issue)
    {
        _text = _text.Replace(issue.Original, issue.Suggested, StringComparison.InvariantCulture);

        _issues.Remove(issue);
    }

    private void ApplyAllFixes()
    {
        foreach (var issue in _issues)
        {
            _text = _text.Replace(issue.Original, issue.Suggested, StringComparison.InvariantCulture);
        }

        _issues.Clear();
    }

    private static Color GetColor(string category)
    {
        return category.ToUpperInvariant() switch
        {
            "GRAMMAR" => Color.Error,
            "SPELLING" => Color.Warning,
            "PUNCTUATION" => Color.Info,
            "STYLE" => Color.Secondary,
            _ => Color.Default
        };
    }

    public record AnalyzeTextRequest(string Text);

    public record AnalyzeTextResponse(
        string OriginalText,
        TextIssueDto[] Issues);

    public record TextIssueDto(
        Guid Id,
        string Original,
        string Suggested,
        string Explanation,
        string Category,
        int StartIndex,
        int Length);

    public record TextIssueVm(
        Guid Id,
        string Original,
        string Suggested,
        string Explanation,
        string Category,
        int StartIndex,
        int Length);
}

