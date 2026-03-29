using AskAgainApi.Entity.PollResult;

namespace AskAgainApi.Repositories
{
    public interface IPollResultRepository
    {
        public Task CreateAsync(PollResultEntity pollResult);
        public Task RemoveAsync(Guid pollId);
        public Task AddUserToOptionAsync(Guid userId, Guid sessionId, Guid pollId, Guid optionId);
        public Task<PollResultEntity?> GetByPollIdAsync(Guid pollId);
        public Task<ICollection<PollResultEntity>> GetBySessionIdAsync(Guid sessionId);
        public Task SetStartTime(Guid pollId, DateTime startTime);
        public Task SetEndTime(Guid pollId, DateTime? endTime);
    }
}
