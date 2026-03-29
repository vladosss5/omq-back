using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.Session.Request;
using AskAgainApi.Models.DTO.Session.Response;

namespace AskAgainApi.Services
{
    public interface ISessionService
    {
        public Task<SessionResponseDTO> CreateAsync(SessionCreateDTO createSessionDTO, Guid userId);
        public Task UpdateAsync(SessionUpdateDTO updateSessionDTO, Guid userId);
        public Task<SessionGuestResponseDTO> GetGuestAsync(Guid sessionId);
        public Task<SessionGuestResponseDTO> GetGuestByShortIdAsync(int sessionId);
        public Task<Guid?> GetFullIdByShortIdAsync(int sessionId);
        public Task<SessionResponseDTO> GetAsync(Guid sessionId);
        public Task<ICollection<SessionPreviewResponseDTO>> GetUserSessionsAsync(Guid userId);
        public Task RemoveAsync(Guid sessionId, Guid userId);
        public Task SetStartDatetimeAsync(SessionStartSetDTO startDatetime, Guid userId);
        public Task FinishAsync(Guid sessionId);
    }
}
