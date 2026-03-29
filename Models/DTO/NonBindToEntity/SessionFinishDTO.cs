using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class SessionFinishDTO
    {
        [Required]
        public Guid SessionId { get; set; }
    }
}
