namespace AskAgainApi.Services
{
    public interface ISessionPrivilegesService
    {
        public Task CheckAccess(Guid SessionId, Guid userId);
    }
}
