using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class SessionStartSetDTO
    {
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public DateTime StartDatetime { get; set; }
    }
}
