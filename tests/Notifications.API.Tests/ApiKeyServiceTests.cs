using Microsoft.EntityFrameworkCore;
using Notifications.API.Database;
using Notifications.API.Models;
using Notifications.API.Services.ApiKey;

namespace Notifications.API.Tests;

public class ApiKeyServiceTests : IDisposable
{
    private readonly NotificationsDbContext _dbContext;
    private readonly ApiKeyService _sut;

    public ApiKeyServiceTests()
    {
        var options = new DbContextOptionsBuilder<NotificationsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new NotificationsDbContext(options);
        _sut = new ApiKeyService(_dbContext);
    }

    [Fact]
    public async Task IsApiKeyValid_KeyExists_ReturnsTrue()
    {
        _dbContext.ApiKeys.Add(new ApiKey { Key = "valid-key" });
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.IsApiKeyValid("valid-key");

        Assert.True(result);
    }

    [Fact]
    public async Task IsApiKeyValid_KeyDoesNotExist_ReturnsFalse()
    {
        var result = await _sut.IsApiKeyValid("non-existent-key");

        Assert.False(result);
    }

    [Fact]
    public async Task IsApiKeyValid_WrongKey_ReturnsFalse()
    {
        _dbContext.ApiKeys.Add(new ApiKey { Key = "correct-key" });
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.IsApiKeyValid("wrong-key");

        Assert.False(result);
    }

    [Fact]
    public async Task IsApiKeyValid_KeyIsCaseSensitive_ReturnsFalse()
    {
        _dbContext.ApiKeys.Add(new ApiKey { Key = "MySecretKey" });
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.IsApiKeyValid("mysecretkey");

        Assert.False(result);
    }

    [Fact]
    public async Task IsApiKeyValid_MultipleKeysExist_MatchesCorrectOne()
    {
        _dbContext.ApiKeys.AddRange(
            new ApiKey { Key = "key-one" },
            new ApiKey { Key = "key-two" },
            new ApiKey { Key = "key-three" }
        );
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.IsApiKeyValid("key-two");

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task IsApiKeyValid_EmptyOrWhitespaceKey_ReturnsFalse(string apiKey)
    {
        _dbContext.ApiKeys.Add(new ApiKey { Key = "valid-key" });
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.IsApiKeyValid(apiKey);

        Assert.False(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
