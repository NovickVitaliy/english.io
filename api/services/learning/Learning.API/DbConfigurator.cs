using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Learning;

public static class DbConfigurator
{
    public static Task ConfigureDatabase(this WebApplication app)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return Task.CompletedTask;
    }
}