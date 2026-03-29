using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Session.Request
{
    public class SessionUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public SessionSettingsUpdateDTO Settings { get; set; }
    }
}
