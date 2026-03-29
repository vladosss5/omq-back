namespace AskAgainApi.Models.DTO.User.Response
{
    public class UserResponseDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Login { get; set; }

        public string? TariffId { get; set; }

        public int CountSessionsAvailableToCreate { get; set; }

        public ICollection<UserLikeQuestionsInSessionResponseDTO> LikeQuestionIds { get; set; } = new List<UserLikeQuestionsInSessionResponseDTO>();

        public ICollection<SessionOrgResponseDTO>? OrgSessions { get; set; }

    }
}
