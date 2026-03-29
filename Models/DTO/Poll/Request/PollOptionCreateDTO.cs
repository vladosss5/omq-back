using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Poll.Request
{
    public class PollOptionCreateDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

    }
}
