using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class AuthUserDTO
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Pass { get; set; }
    }
}
