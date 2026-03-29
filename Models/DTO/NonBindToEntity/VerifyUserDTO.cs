using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class VerifyUserDTO
    {
        [Required]
        public Guid Token { get; set; }
    }
}
