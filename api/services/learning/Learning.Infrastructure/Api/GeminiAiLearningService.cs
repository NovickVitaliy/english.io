using System.Text.Json;
using Learning.Application.Contracts.Api;
using Learning.Application.DTOs.Decks;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Learning.Infrastructure.Api;

public class GeminiAiLearningService : IAiLearningService
{
    private readonly AiLearningPromptsOptions _options;
    private readonly HttpClient _httpClient;
    private readonly GeminiOptions _geminiOptions;

    public GeminiAiLearningService(
        IOptions<AiLearningPromptsOptions> options,
        IHttpClientFactory httpClientFactory,
        IOptions<GeminiOptions> geminiOptions)
    {
        _httpClient = httpClientFactory.CreateClient(IAiLearningService.HttpClientKey);
        _options = options.Value;
        _geminiOptions = geminiOptions.Value;
    }

    public async Task<DeckWordDto> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences)
    {
        var body = BuildRequestForTranslatingTheWordWithExampleSentences(word, exampleSentences);
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"v1beta/models/gemini-2.0-flash-001:generateContent?key={_geminiOptions.ApiKey}", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(body))
        };

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);
        var textProperty = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var deckWordDto = JsonSerializer.Deserialize<DeckWordDto>(textProperty!, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
        return JsonSerializer.Deserialize<DeckWordDto>(textProperty!, new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true})!;
    }

    public async Task<bool> DoesWordComplyToTheArticle(string word, string topic)
    {
        var body = BuildRequestForCheckingIfWordCompliesToTheTopic(word, topic);
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"v1beta/models/gemini-2.0-flash-001:generateContent?key={_geminiOptions.ApiKey}", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(body))
        };

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);
        var textProperty = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var deckWordDto = JsonSerializer.Deserialize<DoesWordComplyToTheTopicResponse>(textProperty!, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
        return JsonSerializer.Deserialize<DoesWordComplyToTheTopicResponse>(textProperty!, new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true})!.DoesComply;

    }

    public async Task<TranslatedWordResult[]> VerifyWordsTranslations(TranslateWordsRequest request)
    {
        var body = BuildRequestForVerifyingWordsTranslations(request);
        var httpRequest = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"v1beta/models/gemini-2.0-flash-001:generateContent?key={_geminiOptions.ApiKey}", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(body))
        };

        var response = await _httpClient.SendAsync(httpRequest);
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var textProperty = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return JsonSerializer.Deserialize<TranslatedWordResult[]>(textProperty!, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;

    }

    public async Task<SentenceWithGap[]> GenerateSentencesWithGaps(string[] words)
    {
        var request = BuildRequestForGeneratingSentencesWithGaps(words);
        var httpRequest = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(request)),
            RequestUri = new Uri($"v1beta/models/gemini-2.0-flash-001:generateContent?key={_geminiOptions.ApiKey}", UriKind.Relative)
        };

        var httpResponse = await _httpClient.SendAsync(httpRequest);
        var json = await httpResponse.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .ToString();

        return JsonSerializer.Deserialize<SentenceWithGap[]>(text, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true})!;
    }
    public async Task<string> GenerateExampleTextAsync(string[] words)
    {
        var request = BuildRequestForGeneratingExampleText(words);
        var httpRequest = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(request)),
            RequestUri = new Uri($"v1beta/models/gemini-2.0-flash-001:generateContent?key={_geminiOptions.ApiKey}", UriKind.Relative)
        };

        var response = await _httpClient.SendAsync(httpRequest);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var jsonText = jsonDoc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .ToString();

        var exampleText = JsonDocument.Parse(jsonText).RootElement.GetProperty("text").ToString();
        return exampleText;
    }

    private object BuildRequestForGeneratingExampleText(string[] words)
    {
        var prompt = _options.PromptForGeneratingExampleText
            .Replace("{words}", string.Join(',', words), StringComparison.InvariantCultureIgnoreCase);

        return BuildRequestWithPrompt(prompt);
    }

    private object BuildRequestForGeneratingSentencesWithGaps(string[] words)
    {
        var prompt = _options.PromptForGeneratingSentencesWithGaps
            .Replace("{words}", string.Join(',', words), StringComparison.InvariantCulture);

        return BuildRequestWithPrompt(prompt);
    }

    private object BuildRequestForVerifyingWordsTranslations(TranslateWordsRequest request)
    {
        var prompt = _options.PromptForCheckingIfTranslationsAreCorrect
            .Replace("{words}", string.Join("; ", request.TranslatedWords.Select(x => x.OriginalWord)), StringComparison.InvariantCulture)
            .Replace("{originalLanguage}", request.OriginalLanguage, StringComparison.InvariantCulture)
            .Replace("{translatedWords}", string.Join("; ", request.TranslatedWords.Select(x => x.Translated)), StringComparison.InvariantCulture)
            .Replace("{translatedLanguage}", request.TranslatedLanguage, StringComparison.InvariantCulture);

        return BuildRequestWithPrompt(prompt);
    }

    private object BuildRequestForCheckingIfWordCompliesToTheTopic(string word, string topic)
    {
        var prompt = _options.PromptForCheckingIfWordCompliesToTheTopic
            .Replace("{word}", word, StringComparison.InvariantCulture)
            .Replace("{topic}", topic, StringComparison.InvariantCulture);

        return BuildRequestWithPrompt(prompt);
    }

    private static object BuildRequestWithPrompt(string prompt)
    {
        return new
        {
            contents = new object[]
            {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            },
            generationConfig = new
            {
                response_mime_type = "application/json"
            }
        };
    }

    private object BuildRequestForTranslatingTheWordWithExampleSentences(string word, int exampleSentences)
    {
        var prompt = _options.PromptForWordTranslatingWithExampleSentences
            .Replace("{numSentences}", exampleSentences.ToString(), StringComparison.InvariantCulture)
            .Replace("{word}", word, StringComparison.InvariantCulture);

        return BuildRequestWithPrompt(prompt);
    }
}
