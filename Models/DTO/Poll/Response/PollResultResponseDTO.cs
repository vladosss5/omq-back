using AskAgainApi.Entity.PollResult;

namespace AskAgainApi.Models.DTO.Poll.Response
{
    public class PollResultResponseDTO
    {
        public Guid SessionId { get; set; }

        public Guid PollId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public ICollection<PollResultOptionEntity> Options { get; set; } = new List<PollResultOptionEntity>();

    }
}
