using AskAgainApi.Models.DTO.Poll.Response;
using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Poll.Request
{
    public class PullUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid SessionID { get; set; }

        [Required]
        public string Question { get; set; }

        public ICollection<PollOptionResponseDTO> Options { get; set; } = new List<PollOptionResponseDTO>();
    }
}
