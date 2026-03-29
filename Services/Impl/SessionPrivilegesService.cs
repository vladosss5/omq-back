using AskAgainApi.Exceptions;
using AskAgainApi.Repositories;

namespace AskAgainApi.Services.Impl
{
    public class SessionPrivilegesService : ISessionPrivilegesService
    {
        private readonly IUserRepository _userRepository;

        public SessionPrivilegesService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CheckAccess(Guid sessionId, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new HttpException("Auth is bad", 401);

            if (user.OrgSessions.FirstOrDefault(session => session != null && sessionId.Equals(session.Id), null) is null)
                throw new HttpException("Access to this session denied.");
        }

    }
}
