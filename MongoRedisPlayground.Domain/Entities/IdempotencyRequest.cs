using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRedisPlayground.Domain.Entities;

public sealed class IdempotencyRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public string ResponseBody { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}