using System.Text;
using Fluxor;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Shared.Extensions;
using Shared.Store.User;

namespace Learning.Features.DashboardLayout.Components;

public partial class DashboardFooter : ComponentBase, IDisposable
{
    private bool _disposed;

    [Inject] private IStringLocalizer<DashboardFooter> Localizer { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private AiChatService AiChatService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;

    private string Location { get; set; } = null!;

    private bool _drawerOpen;
    private bool _isLoading;

    private string _message = string.Empty;

    private readonly List<ChatMessage> _messages = [];

    private CancellationTokenSource? _streamCancellationTokenSource;

    private readonly List<string> _allSuggestions =
    [
        "Give me synonyms for 'happy'",
        "Explain the word 'achievement'",
        "What is the antonym of 'brave'?",
        "Create a sentence with 'improve'",
        "Explain the difference between 'say' and 'tell'",
        "Give me 5 advanced synonyms for 'important'",
        "What does 'overwhelmed' mean?",
        "Generate a short story using 'journey'",
        "Give me common collocations with 'make'",
        "Explain the word 'persistent'",
        "Create a quiz for the word 'challenge'",
        "Give me informal synonyms for 'friend'",
        "Explain phrasal verb 'give up'",
        "Teach me business English vocabulary",
        "Give me difficult C1 words",
        "Generate 3 sentences with 'although'",
        "Explain when to use 'few' vs 'a few'",
        "What are common mistakes with 'advice'?",
        "Teach me words related to emotions",
        "Generate a mini reading task"
    ];

    private List<string> _randomSuggestions = [];

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;

        if (_drawerOpen)
        {
            GenerateRandomSuggestions();
        }
    }

    private void GenerateRandomSuggestions()
    {
        _randomSuggestions = _allSuggestions
            .OrderBy(_ => Guid.NewGuid())
            .Take(6)
            .ToList();
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_message) || _isLoading)
            return;

        var userMessage = _message;
        _message = string.Empty;

        _messages.Add(new ChatMessage("user", userMessage));

        var assistantMessage = new ChatMessage("assistant", "");
        _messages.Add(assistantMessage);

        var index = _messages.Count - 1;

        _isLoading = true;
        StateHasChanged();

        if (_streamCancellationTokenSource != null)
        {
            await _streamCancellationTokenSource.CancelAsync();
        }

        _streamCancellationTokenSource = new CancellationTokenSource();

        var sb = new StringBuilder();

        try
        {
            await foreach (var chunk in AiChatService.StreamAsync(
                               _messages,
                               UserState.Value.Token,
                               _streamCancellationTokenSource.Token))
            {
                sb.Append(chunk);

                _messages[index] = assistantMessage with
                {
                    Text = sb.ToString()
                };

                await InvokeAsync(StateHasChanged);
            }
        }
        catch (OperationCanceledException)
        {
            _messages[index] = assistantMessage with
            {
                Text = sb + "\n\n[Cancelled]"
            };
        }
        catch (Exception ex)
        {
            _messages[index] = assistantMessage with
            {
                Text = $"Error: {ex.Message}"
            };
        }
        finally
        {
            _isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" && !args.ShiftKey)
        {
            await SendMessage();
        }
    }

    private async Task UseSuggestion(string suggestion)
    {
        _message = suggestion;

        await SendMessage();
    }

    private static MarkupString RenderMarkdown(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new MarkupString("");

        var html = Markdown.ToHtml(text);

        return new MarkupString(html);
    }

    private static string GetMessageStyle(bool isUser)
    {
        return isUser
            ? """
              background-color: var(--mud-palette-primary);
              color: white;
              border-radius: 16px;
              max-width: 80%;
              """
            : """
              background-color: var(--mud-palette-surface);
              border-radius: 16px;
              max-width: 80%;
              """;
    }

    protected override void OnInitialized()
    {
        GenerateRandomSuggestions();

        NavigationManager.LocationChanged += OnLocationChanged;

        Location = NavigationManager.GetRelativePath();

        StateHasChanged();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Location = NavigationManager.GetRelativePath();

        StateHasChanged();
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                NavigationManager.LocationChanged -= OnLocationChanged;

                _streamCancellationTokenSource?.Cancel();

                _streamCancellationTokenSource?.Dispose();
            }

            _disposed = true;
        }
    }

    ~DashboardFooter()
    {
        Dispose(false);
    }
}

public record ChatMessage(string Role, string Text);
