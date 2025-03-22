using Microsoft.Net.Http.Headers;

namespace Shared.DelegateHttpHandlers;

public class LocalizationHttpHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var language = Thread.CurrentThread.CurrentCulture.Name;

        request.Headers.Add(HeaderNames.AcceptLanguage, language);

        return base.SendAsync(request, cancellationToken);
    }
}
