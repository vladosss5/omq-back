using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.Session.Question
{
    public class SessionQuestionEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; } = true;
        public string Text { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public bool IsOrg { get; set; } = false;

        public ICollection<Guid> LikesUsersId { get; set; } = new List<Guid>();
    }
}
