using System.Text.Json;
using Learning.Application.Contracts.Api;
using Learning.Application.DTOs.Decks;
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
        Console.WriteLine(_httpClient.BaseAddress);
        var body = BuildRequest(word, exampleSentences);
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

    private object BuildRequest(string word, int exampleSentences)
    {
        var prompt = _options.PromptForWordTranslatingWithExampleSentences
            .Replace("{numSentences}", exampleSentences.ToString(), StringComparison.InvariantCulture)
            .Replace("{word}", word, StringComparison.InvariantCulture);

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
}
