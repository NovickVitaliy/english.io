using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Learning.Features.DashboardLayout.Components;

public class AiChatService
{
    private readonly HttpClient _http;

    public AiChatService(HttpClient http)
    {
        _http = http;
    }

    public async IAsyncEnumerable<string> StreamAsync(
        List<ChatMessage> messages,
        string token,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/chat")
        {
            Content = JsonContent.Create(new
            {
                messages = messages.Select(m => new
                {
                    role = m.Role,
                    content = m.Text
                })
            })
        };

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            ct);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var reader = new StreamReader(stream);

        var buffer = new char[256];

        while (!reader.EndOfStream)
        {
            var count = await reader.ReadAsync(buffer, 0, buffer.Length);

            if (count > 0)
                yield return new string(buffer, 0, count);
        }
    }
}
