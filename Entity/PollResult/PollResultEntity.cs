using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.PollResult
{
    public class PollResultEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid PollId { get; set; }
        public Guid SessionId { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public ICollection<PollResultOptionEntity> Options { get; set; } = new List<PollResultOptionEntity>();

    }
}
