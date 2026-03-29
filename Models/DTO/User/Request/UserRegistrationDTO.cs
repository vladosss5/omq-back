using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.User.Request
{
    public class UserRegistrationDTO
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Pass { get; set; } = string.Empty;

    }
}
