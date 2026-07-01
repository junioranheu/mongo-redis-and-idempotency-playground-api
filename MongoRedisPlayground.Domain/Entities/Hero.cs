using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRedisPlayground.Domain.Enums;

namespace MongoRedisPlayground.Domain.Entities;

public sealed class Hero
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string AttributeId { get; set; }

    [BsonIgnore]
    public HeroAttribute? Attribute { get; set; }

    public AttackTypeEnum AttackType { get; set; }

    public List<HeroRoleEnum> Roles { get; set; } = [];

    public List<Ability> Abilities { get; set; } = [];

    public Hero(string? id, string name, string attributeId, AttackTypeEnum attackType)
    {
        if (string.IsNullOrEmpty(id))
        {
            id = ObjectId.GenerateNewId().ToString();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.");
        }

        Id = id;
        Name = name;
        AttributeId = attributeId;
        AttackType = attackType;

        Roles = [];
        Abilities = [];
    }
}