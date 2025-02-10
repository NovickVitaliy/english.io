namespace Shared.Options;

public class ClientOptions
{
    public const string ClientOptionsKey = "ClientOptions";
    public string Domain { get; init; } = null!;
    public int HttpPort { get; init; }
    public int HttpsPort { get; init; }
    public bool IsHttps { get; init; }

    public Uri GetClientBaseUrl()
    {
        return IsHttps
            ? new Uri($"https://{Domain}:{HttpsPort}/")
            : new Uri($"http://{Domain}:{HttpPort}/");
    }
}
