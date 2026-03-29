using AskAgainApi.Enums;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Models.DTO.Question.Response;

namespace AskAgainApi.Models.DTO.Session.Response
{
    public class SessionGuestResponseDTO
    {
        public Guid Id { get; set; }
        public int ShortId { get; set; }
        public string Name { get; set; }
        public SessionStateEnum Status { get; set; }
        public SessionSettingsResponseDTO Settings { get; set; }
        public ICollection<PollResponseDTO>? Polls { get; set; }
        public ICollection<QuestionResponseDTO> Questions { get; set; } = new List<QuestionResponseDTO>();
        public DateTime? Start { get; set; } = null;

    }
}
