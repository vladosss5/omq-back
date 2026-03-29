namespace AskAgainApi.Models.DTO.Poll.Response
{
    public class PollResponseDTO
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = string.Empty;
        public string Question { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public ICollection<PollOptionResponseDTO> Options { get; set; } = new List<PollOptionResponseDTO>();
    }
}
