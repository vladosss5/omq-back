using AskAgainApi.Entity.PollResult;
using AskAgainApi.Entity.Session.Poll;
using AskAgainApi.Exceptions;
using AskAgainApi.Models.DTO.Poll.Request;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Repositories;
using AutoMapper;

namespace AskAgainApi.Services.Impl
{
    public class PollService : IPollService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IPollResultRepository _pollResultRepository;
        private readonly IMapper _mapper;

        public PollService(ISessionRepository sessionRepository, IMapper mapper, IPollResultRepository pollResultRepository)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _pollResultRepository = pollResultRepository;
        }

        public async Task<PollResponseDTO> CreateAsync(PollCreateDTO createPollDTO, Guid userId)
        {
            await CheckSessionExist(createPollDTO.SessionId);

            var pollEntity = _mapper.Map<SessionPollEntity>(createPollDTO);

            await _sessionRepository.CreatePollAsync(createPollDTO.SessionId, pollEntity);

            return _mapper.Map<PollResponseDTO>(pollEntity);
        }

        public Task<PollResponseDTO> UpdateAsync(PullUpdateDTO updatePollDTO, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PollResponseDTO> GetAsync(Guid sessionId, Guid pollId)
        {
            var poll = await _sessionRepository.GetPollAsync(sessionId, pollId);

            return poll == null ? throw new HttpException("Poll is not found") : _mapper.Map<PollResponseDTO>(poll);
        }

        private async Task CheckSessionExist(Guid sessionId)
        {
            if (await _sessionRepository.GetByIdAsync(sessionId) == null)
                throw new HttpException("Session is not exist.");
        }

        public async Task RemoveAsync(Guid sessionId, Guid id)
        {
            var questionEntity = await _sessionRepository.GetPollAsync(sessionId, id);

            if (null == questionEntity)
                throw new HttpException("Poll is not exist.");

            await _sessionRepository.RemovePollAsync(sessionId, id);
        }


        public async Task<DateTime> StartPoll(Guid sessionId, Guid pollId)
        {
            var pollResultEntity = await _pollResultRepository.GetByPollIdAsync(pollId);

            if (null == pollResultEntity)
            {
                var pollEntity = await _sessionRepository.GetPollAsync(sessionId, pollId) ?? throw new HttpException("Poll not found");

                pollResultEntity = _mapper.Map<PollResultEntity>(pollEntity);

                pollResultEntity.SessionId = sessionId;

                await _pollResultRepository.CreateAsync(pollResultEntity);
            }

            var now = DateTime.Now;

            await _pollResultRepository.SetStartTime(pollId, now);
            await _pollResultRepository.SetEndTime(pollId, null);

            return now;
        }

        public async Task StopPoll(Guid sessionId, Guid pollId)
        {
            if (null == await _pollResultRepository.GetByPollIdAsync(pollId))
                throw new HttpException("Poll is not started.");

            await _pollResultRepository.SetEndTime(pollId, DateTime.Now);
        }

        public async Task VoteInPoll(PollVoteDTO voteDTO, Guid userId)
        {
            await _pollResultRepository.AddUserToOptionAsync(userId, voteDTO.SessionID, voteDTO.PollID, voteDTO.OptionID);
        }

        public async Task<ICollection<PollResultResponseDTO>> GetResultsForSession(Guid sessionId)
        {
            var entityPolls = await _pollResultRepository.GetBySessionIdAsync(sessionId);

            return entityPolls.Select(_mapper.Map<PollResultResponseDTO>).ToList();
        }
    }
}
