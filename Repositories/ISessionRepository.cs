using AskAgainApi.Entity.Session;
using AskAgainApi.Entity.Session.Poll;
using AskAgainApi.Entity.Session.Question;

namespace AskAgainApi.Repositories
{
    public interface ISessionRepository
    {
        public Task CreateAsync(SessionEntity sessionEntity);
        public Task<SessionEntity?> GetByIdAsync(Guid id);
        public Task<SessionEntity?> GetByShortIdAsync(int shortId);
        public Task UpdateSettingAndNameAsync(Guid sessionId, SessionSettingsEntity settings, string sessionName);
        public Task SetStartSessionAsync(Guid sessionId, DateTime startSession, DateTime endSession);
        public Task SetEndSessionAsync(Guid sessionId, DateTime endSession);
        public Task RemoveAsync(Guid id);
        public Task CreatePollAsync(Guid sessionId, SessionPollEntity pollEntity);
        public Task<SessionPollEntity?> GetPollAsync(Guid sessionId, Guid pollId);
        public Task RemovePollAsync(Guid sessionId, Guid pollId);

        public Task CreateQuestionAsync(Guid sessionId, SessionQuestionEntity questionEntity);
        public Task LikeQuestionAsync(Guid userId, Guid sessionId, Guid questionId);
        public Task DislikeQuestionAsync(Guid userId, Guid sessionId, Guid questionId);
        public Task<SessionQuestionEntity?> GetQuestionAsync(Guid sessionId, Guid questionId);
        public Task RemoveQuestionAsync(Guid sessionId, Guid questionId);
        public Task ArchiveQuestionAsync(Guid sessionId, Guid questionId);
        public Task UnarchiveQuestionAsync(Guid sessionId, Guid questionId);
    }
}
