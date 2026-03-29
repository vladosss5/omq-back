using AskAgainApi.Entity.Session.Question;
using AskAgainApi.Exceptions;
using AskAgainApi.Models.DTO.Question.Request;
using AskAgainApi.Models.DTO.Question.Response;
using AskAgainApi.Repositories;
using AutoMapper;

namespace AskAgainApi.Services.Impl
{
    public class QuestionService : IQuestionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public QuestionService(ISessionRepository sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public async Task<QuestionResponseDTO> CreateMemberAsync(QuestionCreateDTO createQuestionDTO, Guid authorId, string authorName)
        {
            await CheckSessionExist(createQuestionDTO.SessionId);

            var questionEntity = _mapper.Map<SessionQuestionEntity>(createQuestionDTO);

            questionEntity.AuthorId = authorId;
            questionEntity.AuthorName = authorName;
            questionEntity.IsOrg = false;

            await _sessionRepository.CreateQuestionAsync(createQuestionDTO.SessionId, questionEntity);

            return _mapper.Map<QuestionResponseDTO>(questionEntity);

        }

        public async Task<QuestionResponseDTO> CreateOrgAsync(QuestionCreateDTO createQuestionDTO, Guid authorId, string authorName)
        {
            await CheckSessionExist(createQuestionDTO.SessionId);

            var questionEntity = _mapper.Map<SessionQuestionEntity>(createQuestionDTO);

            questionEntity.AuthorId = authorId;
            questionEntity.AuthorName = authorName;
            questionEntity.IsOrg = true;

            await _sessionRepository.CreateQuestionAsync(createQuestionDTO.SessionId, questionEntity);

            return _mapper.Map<QuestionResponseDTO>(questionEntity);
        }

        public async Task<QuestionResponseDTO> LikeQuestionAsync(QuestionLikeDTO likeQuestionDTO, Guid userId)
        {
            await CheckQuestionExist(likeQuestionDTO.SessionId, likeQuestionDTO.QuestionId);

            await _sessionRepository.LikeQuestionAsync(
                userId, likeQuestionDTO.SessionId, likeQuestionDTO.QuestionId);

            return _mapper.Map<QuestionResponseDTO>(
                await _sessionRepository.GetQuestionAsync(
                    likeQuestionDTO.SessionId, likeQuestionDTO.QuestionId));
        }

        private async Task CheckSessionExist(Guid sessionId)
        {
            if (await _sessionRepository.GetByIdAsync(sessionId) == null)
                throw new HttpException("Session is not exist.");
        }

        private async Task CheckQuestionExist(Guid sessionId, Guid questionId)
        {
            if (null == await _sessionRepository.GetQuestionAsync(sessionId, questionId))
                throw new HttpException("Question is not exist.");
        }

        public async Task DeleteAsync(Guid sessionId, Guid id, Guid userId)
        {
            var questionEntity = await _sessionRepository.GetQuestionAsync(sessionId, id);

            if (null == questionEntity)
                throw new HttpException("Question is not exist.");

            if (questionEntity.AuthorId != userId)
                throw new HttpException("You can't delete someone else's question.");

            await _sessionRepository.RemoveQuestionAsync(sessionId, id);
        }

        public async Task DeleteWithoutUserCheckAsync(Guid sessionId, Guid id)
        {
            var questionEntity = await _sessionRepository.GetQuestionAsync(sessionId, id);

            if (null == questionEntity)
                throw new HttpException("Question is not exist.");

            await _sessionRepository.RemoveQuestionAsync(sessionId, id);
        }

        public async Task ArchiveQuestionAsync(Guid sessionId, Guid id)
        {
            var questionEntity = await _sessionRepository.GetQuestionAsync(sessionId, id);

            if (null == questionEntity)
                throw new HttpException("Question is not exist.");

            await _sessionRepository.ArchiveQuestionAsync(sessionId, id);
        }

        public async Task UnarchiveQuestionAsync(Guid sessionId, Guid id)
        {
            var questionEntity = await _sessionRepository.GetQuestionAsync(sessionId, id);

            if (null == questionEntity)
                throw new HttpException("Question is not exist.");

            await _sessionRepository.UnarchiveQuestionAsync(sessionId, id);
        }

        public async Task<QuestionResponseDTO> DislikeQuestionAsync(QuestionLikeDTO dislikeQuestionDTO, Guid userId)
        {
            await CheckQuestionExist(dislikeQuestionDTO.SessionId, dislikeQuestionDTO.QuestionId);

            await _sessionRepository.DislikeQuestionAsync(
                userId, dislikeQuestionDTO.SessionId, dislikeQuestionDTO.QuestionId);

            return _mapper.Map<QuestionResponseDTO>(
                await _sessionRepository.GetQuestionAsync(
                    dislikeQuestionDTO.SessionId, dislikeQuestionDTO.QuestionId));
        }
    }
}
