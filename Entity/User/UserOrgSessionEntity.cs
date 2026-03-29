using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.User
{
    public class UserOrgSessionEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
