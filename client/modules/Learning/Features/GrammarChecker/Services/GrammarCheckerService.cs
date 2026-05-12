using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Learning.Features.GrammarChecker.Services;

public class GrammarCheckerService(HttpClient httpClient)
{
    public async Task<Pages.GrammarChecker.AnalyzeTextResponse?> AnalyzeTextAsync(
        string text,
        string token)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/grammar");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        request.Content = JsonContent.Create(
            new Pages.GrammarChecker.AnalyzeTextRequest(text));

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<Pages.GrammarChecker.AnalyzeTextResponse>();
    }
}
