using System.Net.Http.Json;
using System.Text.Json;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Grammar;
using Microsoft.Extensions.Configuration;

namespace Learning.Infrastructure.Services;

public class GrammarCheckerService : IGrammarCheckerService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GrammarCheckerService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<AnalyzeTextResponse> AnalyzeAsync(string text)
    {
        var apiKey = _configuration["Gemini:ApiKey"]!;

        var prompt = PromptBuilder.BuildGrammarPrompt(text);

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}",
            body);

        response.EnsureSuccessStatusCode();

        var geminiResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

        var rawText = geminiResponse
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var cleanedJson = rawText!
            .Replace("```json", "", StringComparison.InvariantCulture)
            .Replace("```", "", StringComparison.InvariantCulture)
            .Trim();

        var parsed = JsonSerializer.Deserialize<GeminiIssuesResponse>(cleanedJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return new AnalyzeTextResponse(
            text,
            parsed?.Issues
                .Select(x => new TextIssue(
                    Guid.NewGuid(),
                    x.Original,
                    x.Suggested,
                    x.Explanation,
                    x.Category,
                    x.StartIndex,
                    x.Length))
                .ToArray() ?? []);
    }
}

public class GeminiIssuesResponse
{
    public GeminiIssue[] Issues { get; set; } = [];
}

public class GeminiIssue
{
    public string Original { get; set; } = string.Empty;
    public string Suggested { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int StartIndex { get; set; }
    public int Length { get; set; }
}
