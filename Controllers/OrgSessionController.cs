using AskAgainApi.Helpers;
using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.Poll.Request;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Models.DTO.Question.Request;
using AskAgainApi.Models.DTO.Question.Response;
using AskAgainApi.Models.DTO.Session.Request;
using AskAgainApi.Models.DTO.Session.Response;
using AskAgainApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace AskAgainApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrgSessionController : UserApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IQuestionService _questionService;
        private readonly IPollService _pollService;
        private readonly ISessionPrivilegesService _sessionPrivilegesService;

        public OrgSessionController(ISessionService sessionService, IQuestionService questionService, IPollService pollService, ISessionPrivilegesService sessionPrivilegesService)
        {
            _sessionService = sessionService;
            _questionService = questionService;
            _pollService = pollService;
            _sessionPrivilegesService = sessionPrivilegesService;
        }

        [HttpPost]
        public async Task<ActionResult<SessionResponseDTO>> CreateSession(SessionCreateDTO createSessionDTO)
        {
            var session = await _sessionService.CreateAsync(createSessionDTO, GetUserId());

            return session;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateSession(SessionUpdateDTO updateSessionDTO)
        {
            await _sessionPrivilegesService.CheckAccess(updateSessionDTO.Id, GetUserId());

            await _sessionService.UpdateAsync(updateSessionDTO, GetUserId());

            return Ok();
        }

        [HttpGet("my")]
        public async Task<ActionResult<ICollection<SessionPreviewResponseDTO>>> GetAllHaveSessions()
        {
            var sessions = await _sessionService.GetUserSessionsAsync(GetUserId());

            return Ok(sessions);
        }


        [HttpGet("{id:length(36)}")]
        public async Task<ActionResult<SessionResponseDTO>> GetSession(Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(id, GetUserId());

            var session = await _sessionService.GetAsync(id);

            return session;
        }

        [HttpDelete("{id:length(36)}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(id, GetUserId());

            await _sessionService.RemoveAsync(id, GetUserId());

            return Ok();
        }

        [HttpPost("poll")]
        public async Task<ActionResult<PollResponseDTO>> CreatePoll(PollCreateDTO createPollDTO)
        {
            await _sessionPrivilegesService.CheckAccess(createPollDTO.SessionId, GetUserId());

            var poll = await _pollService.CreateAsync(createPollDTO, GetUserId());

            return poll;
        }

        [HttpGet("{sessionId:length(36)}/poll/{id:length(36)}")]
        public async Task<ActionResult<PollResponseDTO>> GetPoll(Guid sessionId, Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            var poll = await _pollService.GetAsync(sessionId, id);

            return poll;
        }

        [HttpDelete("{sessionId:length(36)}/poll/{id:length(36)}")]
        public async Task<IActionResult> DeletePoll(Guid sessionId, Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            await _pollService.RemoveAsync(sessionId, id);

            return Ok();
        }


        [HttpPost("question")]
        public async Task<ActionResult<QuestionResponseDTO>> CreateQuestion(QuestionCreateDTO createQuestionDTO)
        {
            await _sessionPrivilegesService.CheckAccess(createQuestionDTO.SessionId, GetUserId());

            var question = await _questionService.CreateOrgAsync(createQuestionDTO, GetUserId(), "Организатор");

            return question;
        }

        [HttpPatch("{sessionId:length(36)}/question/{id:length(36)}/archive")]
        public async Task<ActionResult<QuestionResponseDTO>> ArchiveQuestion(Guid sessionId, Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            await _questionService.ArchiveQuestionAsync(sessionId, id);

            return Ok();
        }


        [HttpPatch("{sessionId:length(36)}/question/{id:length(36)}/unarchive")]
        public async Task<ActionResult<QuestionResponseDTO>> UnarchiveQuestion(Guid sessionId, Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            await _questionService.UnarchiveQuestionAsync(sessionId, id);

            return Ok();
        }


        [HttpDelete("{sessionId:length(36)}/question/{id:length(36)}")]
        public async Task<IActionResult> DeleteQuestion(Guid sessionId, Guid id)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            await _questionService.DeleteAsync(sessionId, id, GetUserId());

            return Ok();

        }

        [HttpGet("{sessionId:length(36)}/poll/result/")]
        public async Task<ActionResult<Collection<PollResultResponseDTO>>> GetPollResults(Guid sessionId)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            var results = await _pollService.GetResultsForSession(sessionId);

            return Ok(results);
        }

        [HttpPost("poll/start")]
        public async Task<ActionResult<DateTime>> StartPoll(PollStartDTO startPoll)
        {
            await _sessionPrivilegesService.CheckAccess(startPoll.SessionId, GetUserId());

            var startDate = await _pollService.StartPoll(startPoll.SessionId, startPoll.PollId);

            return startDate;
        }

        [HttpPost("poll/stop")]
        public async Task<ActionResult> StopPoll(PollStopDTO stopPoll)
        {
            await _sessionPrivilegesService.CheckAccess(stopPoll.SessionId, GetUserId());

            await _pollService.StopPoll(stopPoll.SessionId, stopPoll.PollId);

            return Ok();
        }

        [HttpPatch("{sessionId:length(36)}/start")]
        public async Task<IActionResult> SetSessionStartDatetime(Guid sessionId)
        {
            var userId = GetUserId();
            await _sessionPrivilegesService.CheckAccess(sessionId, userId);

            await _sessionService.SetStartDatetimeAsync(
                new SessionStartSetDTO
                {
                    SessionId = sessionId, 
                    StartDatetime = DateTime.Now
                },
                userId);

            return Ok();
        }

        [HttpPatch("{sessionId:length(36)}/finish")]
        public async Task<IActionResult> FinishSession(Guid sessionId)
        {
            await _sessionPrivilegesService.CheckAccess(sessionId, GetUserId());

            await _sessionService.FinishAsync(sessionId);

            return Ok();
        }

    }
}