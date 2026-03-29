using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class LoginResponseDTO
    {
        [Required]
        public string Token { get; }

        public LoginResponseDTO(string token)
        {
            Token = token;
        }
    }
}
