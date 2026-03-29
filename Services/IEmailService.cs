namespace AskAgainApi.Services
{
    public interface IEmailService
    {
        public Task SendMessegeAsync(string address, string subject, string body);
    }
}
