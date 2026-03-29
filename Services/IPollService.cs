using AskAgainApi.Models.DTO.Poll.Request;
using AskAgainApi.Models.DTO.Poll.Response;

namespace AskAgainApi.Services
{
    public interface IPollService
    {
        public Task<PollResponseDTO> CreateAsync(PollCreateDTO createPollDTO, Guid userId);
        public Task<PollResponseDTO> UpdateAsync(PullUpdateDTO updatePollDTO, Guid userId);
        public Task<PollResponseDTO> GetAsync(Guid sessionId, Guid pollId);
        public Task RemoveAsync(Guid sessionId, Guid id);

        public Task<ICollection<PollResultResponseDTO>> GetResultsForSession(Guid sessionId);
        public Task<DateTime> StartPoll(Guid sessionId, Guid pollId);
        public Task StopPoll(Guid sessionId, Guid pollId);
        public Task VoteInPoll(PollVoteDTO voteDTO, Guid userId);
    }
}
