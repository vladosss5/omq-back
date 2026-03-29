using AskAgainApi.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.User
{
    public class UserEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        public bool Verifyed { get; set; } = false;

        public string Name { get; set; } = String.Empty;

        public string Login { get; set; }

        public string Pass { get; set; }

        public TariffEnum? TariffId { get; set; } = TariffEnum.None;

        public int CountSessionsAvailableToCreate { get; set; } = 1;

        public ICollection<UserLikeQuestionsInSessionsEntity> LikeQuestionIds { get; set; } = new List<UserLikeQuestionsInSessionsEntity>();

        public ICollection<UserOrgSessionEntity> OrgSessions { get; set; } = new List<UserOrgSessionEntity>();
    }
}
