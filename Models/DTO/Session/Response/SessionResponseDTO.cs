using AskAgainApi.Enums;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Models.DTO.Question.Response;

namespace AskAgainApi.Models.DTO.Session.Response
{
    public class SessionResponseDTO
    {
        public string? Id { get; set; }
        public int ShortId { get; set; }
        public string? Name { get; set; }
        public SessionSettingsResponseDTO Settings { get; set; }
        public SessionStateEnum Status { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public ICollection<PollResponseDTO>? Polls { get; set; }
        public ICollection<QuestionResponseDTO>? Questions { get; set; }
    }
}
