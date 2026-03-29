using AskAgainApi.Models.DTO.Question.Request;
using AskAgainApi.Models.DTO.Question.Response;

namespace AskAgainApi.Services
{
    public interface IQuestionService
    {
        public Task<QuestionResponseDTO> CreateOrgAsync(QuestionCreateDTO createQuestionDTO, Guid authorId, string authorName);
        public Task<QuestionResponseDTO> CreateMemberAsync(QuestionCreateDTO createQuestionDTO, Guid authorId, string authorName);
        public Task<QuestionResponseDTO> LikeQuestionAsync(QuestionLikeDTO likeQuestionDTO, Guid userId);
        public Task<QuestionResponseDTO> DislikeQuestionAsync(QuestionLikeDTO dislikeQuestionDTO, Guid userId);

        public Task ArchiveQuestionAsync(Guid sessionId, Guid questionId);
        public Task UnarchiveQuestionAsync(Guid sessionId, Guid questionId);
        public Task DeleteAsync(Guid sessionId, Guid id, Guid userId);
        public Task DeleteWithoutUserCheckAsync(Guid sessionId, Guid id);
    }
}
