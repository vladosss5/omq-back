using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Poll.Request
{
    public class PollVoteDTO
    {
        [Required]
        public Guid PollID { get; set; }

        [Required]
        public Guid SessionID { get; set; }
        [Required]
        public Guid OptionID { get; set; }
    }
}
