using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Notifications.API.Authentication;
using Notifications.API.Services.ApiKey;

namespace Notifications.API.Tests;

public class ApiKeyAuthenticationHandlerTests
{
    private readonly Mock<IApiKeyService> _apiKeyServiceMock;
    private readonly Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _optionsMock;
    private readonly Mock<ILoggerFactory> _loggerMock;
    private readonly UrlEncoder _urlEncoder;
    private const string SchemeName = "ApiKey";
    private const string ValidApiKey = "valid-api-key-123";

    public ApiKeyAuthenticationHandlerTests()
    {
        _apiKeyServiceMock = new Mock<IApiKeyService>();
        _optionsMock = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
        _loggerMock = new Mock<ILoggerFactory>();
        _urlEncoder = UrlEncoder.Default;

        _optionsMock
            .Setup(o => o.Get(It.IsAny<string>()))
            .Returns(new AuthenticationSchemeOptions());

        _loggerMock
            .Setup(l => l.CreateLogger(It.IsAny<string>()))
            .Returns(new Mock<ILogger>().Object);
    }

    private async Task<ApiKeyAuthenticationHandler> CreateHandlerAsync(HttpContext context)
    {
        var handler = new ApiKeyAuthenticationHandler(
            _optionsMock.Object,
            _loggerMock.Object,
            _urlEncoder,
            _apiKeyServiceMock.Object
        );

        await handler.InitializeAsync(
            new AuthenticationScheme(SchemeName, null, typeof(ApiKeyAuthenticationHandler)),
            context
        );

        return handler;
    }

    private static DefaultHttpContext CreateHttpContext(string? apiKey = null)
    {
        var context = new DefaultHttpContext();

        if (apiKey is not null)
            context.Request.Headers["X-API-KEY"] = apiKey;

        return context;
    }

    [Fact]
    public async Task HandleAuthenticateAsync_MissingApiKeyHeader_ReturnsFailure()
    {
        var context = CreateHttpContext();
        var handler = await CreateHandlerAsync(context);

        var result = await handler.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Api key was not provided", result.Failure?.Message);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_InvalidApiKey_ReturnsFailure()
    {
        _apiKeyServiceMock
            .Setup(s => s.IsApiKeyValid(It.IsAny<string>()))
            .ReturnsAsync(false);

        var context = CreateHttpContext("invalid-key");
        var handler = await CreateHandlerAsync(context);

        var result = await handler.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Api key is not valid", result.Failure?.Message);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ValidApiKey_ReturnsSuccess()
    {
        _apiKeyServiceMock
            .Setup(s => s.IsApiKeyValid(ValidApiKey))
            .ReturnsAsync(true);

        var context = CreateHttpContext(ValidApiKey);
        var handler = await CreateHandlerAsync(context);

        var result = await handler.AuthenticateAsync();

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Ticket);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ValidApiKey_SetsCorrectClaims()
    {
        _apiKeyServiceMock
            .Setup(s => s.IsApiKeyValid(ValidApiKey))
            .ReturnsAsync(true);

        var context = CreateHttpContext(ValidApiKey);
        var handler = await CreateHandlerAsync(context);

        var result = await handler.AuthenticateAsync();

        var principal = result.Ticket!.Principal;
        Assert.Equal("Microservice", principal.Identity?.Name);
        Assert.True(principal.IsInRole("Service"));
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ValidApiKey_SetsCorrectSchemeName()
    {
        _apiKeyServiceMock
            .Setup(s => s.IsApiKeyValid(ValidApiKey))
            .ReturnsAsync(true);

        var context = CreateHttpContext(ValidApiKey);
        var handler = await CreateHandlerAsync(context);

        var result = await handler.AuthenticateAsync();

        Assert.Equal(SchemeName, result.Ticket!.AuthenticationScheme);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ValidApiKey_CallsApiKeyServiceOnce()
    {
        _apiKeyServiceMock
            .Setup(s => s.IsApiKeyValid(ValidApiKey))
            .ReturnsAsync(true);

        var context = CreateHttpContext(ValidApiKey);
        var handler = await CreateHandlerAsync(context);

        await handler.AuthenticateAsync();

        _apiKeyServiceMock.Verify(s => s.IsApiKeyValid(ValidApiKey), Times.Once);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_MissingApiKeyHeader_NeverCallsApiKeyService()
    {
        var context = CreateHttpContext();
        var handler = await CreateHandlerAsync(context);

        await handler.AuthenticateAsync();

        _apiKeyServiceMock.Verify(s => s.IsApiKeyValid(It.IsAny<string>()), Times.Never);
    }
}
