using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class WordDialog : ComponentBase
{
    private ElementReference _audioElement;
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Parameter] public DeckWordDto DeckWord { get; init; } = null!;
    [Inject] private IStringLocalizer<WordDialog> Localizer { get; init; } = null!;
    [Inject] private ITextToSpeechService TextToSpeechService { get; init; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; init; } = null!;

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private async Task PlayWord(string word, string language)
    {
        try
        {
            var response = await TextToSpeechService.GetWordAsSpeech(word, language);
            var stream = await response.Content.ReadAsStreamAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var base64String = Convert.ToBase64String(memoryStream.ToArray());
            var audioSrc = $"data:audio/mp3;base64,{base64String}";

            await JsRuntime.InvokeVoidAsync("playAudio", _audioElement, audioSrc);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

