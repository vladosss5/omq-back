using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.Session.Poll
{
    public class SessionPollEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public ICollection<SessionPollOptionEntity> Options { get; set; } = new List<SessionPollOptionEntity>();
    }
}
