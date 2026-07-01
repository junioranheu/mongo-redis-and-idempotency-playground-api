using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRedisPlayground.Domain.Entities;

public sealed class HeroAttribute
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
}