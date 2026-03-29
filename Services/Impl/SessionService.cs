using AskAgainApi.Entity.Session;
using AskAgainApi.Entity.User;
using AskAgainApi.Exceptions;
using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.Session.Request;
using AskAgainApi.Models.DTO.Session.Response;
using AskAgainApi.Repositories;
using AutoMapper;

namespace AskAgainApi.Services.Impl
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public SessionService(
            ISessionRepository sessionRepository, 
            IUserRepository userRepository, 
            IMapper mapper, 
            IConfiguration configuration)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<SessionResponseDTO> CreateAsync(SessionCreateDTO createSessionDTO, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new HttpException("Auth is not valid", 401);
            
            var isMonetizationEnabled = _configuration.GetValue<bool>("Monetization");

            if (isMonetizationEnabled && user.CountSessionsAvailableToCreate <= 0)
            {
                throw new HttpException("Count sessions available to create is 0");
            }

            var sessionEntity = _mapper.Map<SessionEntity>(createSessionDTO);

            await _sessionRepository.CreateAsync(sessionEntity);

            var orgSessionEntity = _mapper.Map<UserOrgSessionEntity>(sessionEntity);

            await _userRepository.AddSessionToUserAsync(userId, orgSessionEntity);

            if(isMonetizationEnabled) 
                await _userRepository.IncrementCountOfAvailableToCreateSessions(userId, -1);

            return _mapper.Map<SessionResponseDTO>(sessionEntity);
        }

        public async Task RemoveAsync(Guid sessionId, Guid userId)
        {
            await CheckExist(sessionId);

            await _userRepository.RemoveAccessToSessionAsync(sessionId, userId);
            await _sessionRepository.RemoveAsync(sessionId);
        }

        public async Task<SessionResponseDTO> GetAsync(Guid sessionId)
        {
            var sessionEntity = await CheckExist(sessionId);

            return _mapper.Map<SessionResponseDTO>(sessionEntity);
        }

        public async Task<SessionGuestResponseDTO> GetGuestAsync(Guid sessionId)
        {
            return _mapper.Map<SessionGuestResponseDTO>(await _sessionRepository.GetByIdAsync(sessionId));
        }
        public async Task<SessionGuestResponseDTO> GetGuestByShortIdAsync(int sessionId)
        {
            return _mapper.Map<SessionGuestResponseDTO>(await _sessionRepository.GetByShortIdAsync(sessionId));
        }

        public async Task UpdateAsync(SessionUpdateDTO updateSessionDTO, Guid userId)
        {
            await CheckExist(updateSessionDTO.Id);

            await _sessionRepository.UpdateSettingAndNameAsync(
                updateSessionDTO.Id, _mapper.Map<SessionSettingsEntity>(updateSessionDTO.Settings), updateSessionDTO.Name);

            await _userRepository.UpdateSessionToUserAsync(userId, _mapper.Map<UserOrgSessionEntity>(updateSessionDTO));
        }

        public async Task<ICollection<SessionPreviewResponseDTO>> GetUserSessionsAsync(Guid userId)
        {
            var sessions = await _userRepository.GetOrgSessionsAsync(userId) ?? throw new HttpException("User not exist.", 401);

            return _mapper.Map<ICollection<SessionPreviewResponseDTO>>(sessions);
        }

        public async Task SetStartDatetimeAsync(SessionStartSetDTO sessionStartSetDTO, Guid userId)
        {
            await CheckExist(sessionStartSetDTO.SessionId);
            
            var maxCountActiveSession = _configuration.GetValue<int>("MaxCountActiveSession");
            var countActiveSession = await _userRepository.GetCountActiveSessionByUserIdAsync(userId);

            if (maxCountActiveSession <= countActiveSession)
                throw new HttpException("The maximum number of active sessions has been reached.", 404);

            var startingSession = await _sessionRepository.GetByIdAsync(sessionStartSetDTO.SessionId);
            var possibleExtendSession = _configuration.GetValue<bool>("PossibleExtendSession");
            
            if (startingSession!.End <= sessionStartSetDTO.StartDatetime.ToUniversalTime() && possibleExtendSession)
                throw new HttpException("The session cannot be extended.", 404);
            
            var sessionDurationMonths = _configuration.GetValue<double>("SessionLifetime");
            var endDatetime = sessionStartSetDTO.StartDatetime.AddHours(sessionDurationMonths);

            await _sessionRepository.SetStartSessionAsync(
                sessionStartSetDTO.SessionId, 
                sessionStartSetDTO.StartDatetime, 
                endDatetime);
        }
        
        public async Task FinishAsync(Guid sessionId)
        {
            await CheckExist(sessionId);

            await _sessionRepository.SetEndSessionAsync(sessionId, DateTime.Now);
        }

        private async Task<SessionEntity> CheckExist(Guid sessionId)
        {
            var sessionEntity = await _sessionRepository.GetByIdAsync(sessionId);

            return sessionEntity ?? throw new HttpException("Session is not exist.", 404);
        }

        public async Task<Guid?> GetFullIdByShortIdAsync(int sessionId)
        {
            return (await _sessionRepository.GetByShortIdAsync(sessionId))?.Id;
        }
    }
}
