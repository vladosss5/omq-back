using AskAgainApi.Exceptions;
using AskAgainApi.Helpers;
using AskAgainApi.Models.DTO.Poll.Request;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Models.DTO.Question.Request;
using AskAgainApi.Models.DTO.Question.Response;
using AskAgainApi.Models.DTO.Session.Response;
using AskAgainApi.Services;
using AskAgainApi.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace AskAgainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberSessionController : UserApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IQuestionService _questionService;
        private readonly IPollService _pollService;

        public MemberSessionController(ISessionService sessionService, IQuestionService questionService, IPollService pollService)
        {
            _sessionService = sessionService;
            _questionService = questionService;
            _pollService = pollService;
        }

        [ProducesResponseType(typeof(SessionGuestResponseDTO), 200)]
        [HttpGet("{id:length(36)}")]
        public async Task<ActionResult<SessionGuestResponseDTO>> GetSession(Guid id)
        {
            var session = await _sessionService.GetGuestAsync(id);
            if (session is null)
                throw new HttpException("Session is not found.", 404);

            return session;

        }

        [HttpGet("by-short/{id}")]
        public async Task<ActionResult<Guid>> GetSessionIdByShortId(int id)
        {
            var session = await _sessionService.GetFullIdByShortIdAsync(id);

            if (session is null)
                throw new HttpException("Session is not found.", 404);

            return session;
        }

        [Authorize]
        [ProducesResponseType(typeof(QuestionResponseDTO), 201)]
        [HttpPost("question")]
        public async Task<ActionResult<QuestionResponseDTO>> CreateQuestion(QuestionCreateDTO createQuestionDTO)
        {
            var question = await _questionService.CreateMemberAsync(createQuestionDTO, GetUserId(), GetUserEmail());
            return question;

        }

        [Authorize]
        [HttpDelete("{sessionId:length(36)}/question/{id:length(36)}")]
        public async Task<IActionResult> DeleteQuestion(Guid sessionId, Guid id)
        {
            await _questionService.DeleteAsync(sessionId, id, GetUserId());
            return Ok();
        }


        [Authorize]
        [ProducesResponseType(typeof(QuestionResponseDTO), 200)]
        [HttpPost("question/like")]
        public async Task<ActionResult<QuestionResponseDTO>> LikeQuestion(QuestionLikeDTO likeQuestionDTO)
        {
            var question = await _questionService.LikeQuestionAsync(likeQuestionDTO, GetUserId());
            return question;
        }

        [Authorize]
        [ProducesResponseType(typeof(QuestionResponseDTO), 200)]
        [HttpPost("question/dislike")]
        public async Task<ActionResult<QuestionResponseDTO>> DislikeQuestion(QuestionLikeDTO dislikeQuestionDTO)
        {
            var question = await _questionService.DislikeQuestionAsync(dislikeQuestionDTO, GetUserId());
            return question;
        }


        [HttpGet("{sessionId:length(36)}/poll/result/")]
        public async Task<ActionResult<Collection<PollResultResponseDTO>>> GetPollResults(Guid sessionId)
        {
            var results = await _pollService.GetResultsForSession(sessionId);

            return Ok(results);
        }


        [Authorize]
        [ProducesResponseType(typeof(QuestionResponseDTO), 200)]
        [HttpPost("poll/vote")]
        public async Task<ActionResult> VoteInPoll(PollVoteDTO pollVoteDTO)
        {
            await _pollService.VoteInPoll(pollVoteDTO, GetUserId());
            return Ok();
        }

    }
}
