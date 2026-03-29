using AskAgainApi.Entity.PollResult;
using AskAgainApi.Entity.Session;
using AskAgainApi.Entity.Session.Poll;
using AskAgainApi.Entity.Session.Question;
using AskAgainApi.Entity.User;
using AskAgainApi.Models.DTO.Poll.Request;
using AskAgainApi.Models.DTO.Poll.Response;
using AskAgainApi.Models.DTO.Question.Request;
using AskAgainApi.Models.DTO.Question.Response;
using AskAgainApi.Models.DTO.Session.Request;
using AskAgainApi.Models.DTO.Session.Response;
using AskAgainApi.Models.DTO.User.Request;
using AskAgainApi.Models.DTO.User.Response;
using AutoMapper;

namespace AskAgainApi.Mappers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<SessionEntity, SessionResponseDTO>();
            CreateMap<SessionEntity, SessionGuestResponseDTO>();
            CreateMap<SessionCreateDTO, SessionEntity>();
            CreateMap<SessionEntity, UserOrgSessionEntity>();
            CreateMap<UserOrgSessionEntity, SessionPreviewResponseDTO>();

            CreateMap<SessionPollEntity, PollResponseDTO>();
            CreateMap<PollCreateDTO, SessionPollEntity>();
            CreateMap<PollOptionCreateDTO, SessionPollOptionEntity>();
            CreateMap<SessionPollOptionEntity, PollOptionResponseDTO>();
            CreateMap<SessionSettingsEntity, SessionSettingsResponseDTO>();
            CreateMap<SessionSettingsUpdateDTO, SessionSettingsEntity>();
            CreateMap<SessionUpdateDTO, UserOrgSessionEntity>();

            CreateMap<SessionQuestionEntity, QuestionResponseDTO>();
            CreateMap<QuestionCreateDTO, SessionQuestionEntity>();

            CreateMap<UserEntity, UserResponseDTO>();

            CreateMap<UserRegistrationDTO, UserEntity>();

            CreateMap<SessionPollEntity, PollResultEntity>().ForMember(dist => dist.PollId, (opt) => opt.MapFrom(src => src.Id));
            CreateMap<SessionPollOptionEntity, PollResultOptionEntity>();

            CreateMap<PollResultEntity, PollResultResponseDTO>();
        }
    }
}
