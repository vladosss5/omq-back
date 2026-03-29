using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Poll.Request
{
    public class PollCreateDTO
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public ICollection<PollOptionCreateDTO> Options { get; set; }
    }
}
