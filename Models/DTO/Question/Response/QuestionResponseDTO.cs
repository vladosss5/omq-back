namespace AskAgainApi.Models.DTO.Question.Response
{
    public class QuestionResponseDTO
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public ICollection<Guid> LikesUsersId { get; set; }
    }
}
