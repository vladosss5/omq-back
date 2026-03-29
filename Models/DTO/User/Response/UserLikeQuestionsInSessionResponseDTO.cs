namespace AskAgainApi.Models.DTO.User.Response
{
    public class UserLikeQuestionsInSessionResponseDTO
    {
        public string? SessionId { get; set; }
        public ICollection<string> QuestionIds { get; set; } = new List<string>();
    }
}
