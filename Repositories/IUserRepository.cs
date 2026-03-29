using AskAgainApi.Entity.User;
using AskAgainApi.Enums;

namespace AskAgainApi.Repositories
{
    public interface IUserRepository
    {
        public Task CreateAsync(UserEntity user);
        public Task VerifyAsync(Guid userId);
        public Task UpdateAsync(UserEntity user);
        public Task UpdatePasswordAsync(Guid userId, string password);
        public Task ChangeTariffAsync(Guid userId, TariffEnum tariff);
        public Task IncrementCountOfAvailableToCreateSessions(Guid userId, int count = 1);
        public Task<UserEntity?> GetByIdAsync(Guid id);
        public Task<UserEntity?> GetByLoginAsync(string login);
        public Task AddSessionToUserAsync(Guid userId, UserOrgSessionEntity orgSessionEntity);
        public Task UpdateSessionToUserAsync(Guid userId, UserOrgSessionEntity orgSessionEntity);
        public Task<ICollection<UserOrgSessionEntity>?> GetOrgSessionsAsync(Guid userId);
        public Task RemoveAccessToSessionAsync(Guid sessionId, Guid userId);
        public Task<int> GetCountActiveSessionByUserIdAsync(Guid userId);
    }
}
