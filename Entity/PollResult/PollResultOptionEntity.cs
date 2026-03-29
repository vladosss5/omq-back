using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.PollResult
{
    public class PollResultOptionEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<Guid> Users { get; set; } = new List<Guid>();
    }
}
