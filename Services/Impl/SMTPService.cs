using AskAgainApi.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AskAgainApi.Services.Impl
{
    public class SMTPService : ISMTPService
    {
        private readonly IOptions<EmailSettings> _emailSettings;

        public SMTPService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtp = Activator.CreateInstance<SmtpClient>();
            smtp.Connect(
                _emailSettings.Value.SmtpServer,
                int.Parse(_emailSettings.Value.Port),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(
                _emailSettings.Value.Login,
                _emailSettings.Value.Password);

            return smtp;
        }

        public async Task SendEmailAsync(MimeMessage message)
        {
            var smtp = CreateSmtpClient();
            message.From.Add(MailboxAddress.Parse(_emailSettings.Value.Login));
            await smtp.SendAsync(message);

            smtp.Disconnect(true);
        }
    }
}
