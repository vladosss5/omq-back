namespace AskAgainApi.Models.DTO.Poll.Response
{
    public class PollResultOptionResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<Guid> Users { get; set; } = new List<Guid>();

    }
}
