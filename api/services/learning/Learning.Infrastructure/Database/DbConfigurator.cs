using Learning.Domain;
using Learning.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Learning.Infrastructure.Database;

public static class DbConfigurator
{
    public static async Task ConfigureDatabase(this IHost host)
    {
        ConfigureConventions();
        await ConfigureIndexes(host);
    }

    private static async Task ConfigureIndexes(IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<LearningDbContext>();
        await dbContext.UserPreferences.Indexes.CreateOneAsync(
            new CreateIndexModel<UserPreferences>(Builders<UserPreferences>.IndexKeys.Ascending(x => x.UserEmail), new CreateIndexOptions
            {
                Unique = true,
                Name = "UniqueEmail"
            }));
    }

    private static void ConfigureConventions()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}