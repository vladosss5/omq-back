using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class PollStopDTO
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid PollId { get; set; }

    }
}
