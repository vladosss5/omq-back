using MimeKit;

namespace AskAgainApi.Services
{
    public interface ISMTPService
    {
        public Task SendEmailAsync(MimeMessage message);
    }
}
