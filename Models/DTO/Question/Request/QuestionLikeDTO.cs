using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Question.Request
{
    public class QuestionLikeDTO
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
    }
}
