using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Session.Request
{
    public class SessionCreateDTO
    {
        [Required]
        public string? Name { get; set; }

    }
}
