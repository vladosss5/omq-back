using AskAgainApi.Entity.Session.Poll;
using AskAgainApi.Entity.Session.Question;
using AskAgainApi.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.Session
{
    public class SessionEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public int ShortId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public SessionSettingsEntity Settings { get; set; } = new SessionSettingsEntity();
        public SessionStateEnum Status { get; set; } = SessionStateEnum.NotStarted;
        public DateTime? Start { get; set; } = null;
        public DateTime? End { get; set; } = null;

        public ICollection<SessionPollEntity> Polls { get; set; } = new List<SessionPollEntity>();
        public ICollection<SessionQuestionEntity> Questions { get; set; } = new List<SessionQuestionEntity>();
    }
}
