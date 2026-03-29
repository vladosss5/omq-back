namespace AskAgainApi.Entity.User
{
    public class UserLikeQuestionsInSessionsEntity
    {
        public Guid SessionId { get; set; }

        public ICollection<Guid> QuestionIds { get; set; } = new List<Guid>();
    }
}
