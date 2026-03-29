using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Question.Request
{
    public class QuestionCreateDTO
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
